using Share.Models.AuthDtos;
using Share.Models.UserDtos;

namespace Http.API.Controllers;

/// <summary>
/// 用户账户
/// </summary>
/// <see cref="Application.Manager.UserManager"/>
public class UserController : ClientControllerBase<UserManager>
{
    private readonly CacheService _cache;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;

    public UserController(
        IUserContext user,
        ILogger<UserController> logger,
        UserManager manager,
        CacheService cache,
        IEmailService emailService,
        IConfiguration config) : base(manager, user, logger)
    {
        _cache = cache;
        _emailService = emailService;
        _config = config;
    }

    /// <summary>
    /// 用户注册 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<User>> RegisterAsync(RegisterDto dto)
    {
        // 判断重复用户名
        if (manager.Query.Db.Any(q => q.UserName.Equals(dto.UserName)))
        {
            return Conflict(ErrorMsg.ExistUser);
        }
        // TODO:根据实际需求自定义验证码逻辑
        if (dto.VerifyCode != null)
        {
            if (dto.Email == null)
            {
                return BadRequest("邮箱不能为空");
            }
            var key = AppConst.VerifyCodeCachePrefix + dto.Email;
            var code = _cache.GetValue<string>(key);
            if (code == null)
            {
                return BadRequest("验证码已过期");
            }
            if (!code.Equals(dto.VerifyCode))
            {
                return BadRequest("验证码错误");
            }
        }
        return await manager.RegisterAsync(dto);
    }

    /// <summary>
    /// 注册时，发送邮箱验证码 ✅
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("regVerifyCode")]
    [AllowAnonymous]
    public async Task<ActionResult> SendRegVerifyCodeAsync(string email)
    {
        var captcha = HashCrypto.GetRnd(6);
        var key = AppConst.VerifyCodeCachePrefix + email;
        if (_cache.GetValue<string>(key) != null)
        {
            return Conflict("验证码已发送!");
        }
        // 使用 smtp，可替换成其他
        await _emailService.SendRegisterVerifyAsync(email, captcha);
        // 缓存，默认5分钟过期
        await _cache.SetValueAsync(key, captcha, 60 * 5);
        return Ok();
    }

    /// <summary>
    /// 登录时，发送邮箱验证码 ✅
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("loginVerifyCode")]
    [AllowAnonymous]
    public async Task<ActionResult> SendVerifyCodeAsync(string email)
    {
        if (!manager.Query.Db.Any(q => q.Email != null && q.Email.Equals(email)))
        {
            return NotFound("不存在的邮箱账号");
        }
        var captcha = HashCrypto.GetRnd(6);
        var key = AppConst.VerifyCodeCachePrefix + email;
        if (_cache.GetValue<string>(key) != null)
        {
            return Conflict("验证码已发送!");
        }
        // 使用 smtp，可替换成其他
        await _emailService.SendLoginVerifyAsync(email, captcha);
        // 缓存，默认5分钟过期
        await _cache.SetValueAsync(key, captcha, 60 * 5);

        return Ok();
    }

    /// <summary>
    /// 登录获取Token ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResult>> LoginAsync(LoginDto dto)
    {
        // 查询用户
        User? user = await manager.Query.Db.Where(u => u.UserName.Equals(dto.UserName))
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("不存在该用户");
        }

        // 可将 dto.VerifyCode 设置为必填，以强制验证
        if (dto.VerifyCode != null)
        {
            var key = AppConst.VerifyCodeCachePrefix + user.Email;
            var cacheCode = _cache.GetValue<string>(key);
            if (cacheCode == null)
            {
                return BadRequest("验证码已过期");
            }
            if (!cacheCode.Equals(dto.VerifyCode))
            {
                return BadRequest("验证码错误");
            }
        }
        if (HashCrypto.Validate(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            // 获取Jwt配置
            JwtOption jwtOption = _config.GetSection("Authentication:Jwt").Get<JwtOption>()
                ?? throw new ArgumentNullException("未找到Jwt选项!");
            var sign = jwtOption.Sign;
            var issuer = jwtOption.ValidIssuer;
            var audience = jwtOption.ValidAudiences;

            // 构建返回内容
            if (!string.IsNullOrWhiteSpace(sign) &&
                !string.IsNullOrWhiteSpace(issuer) &&
                !string.IsNullOrWhiteSpace(audience))
            {
                // 设置角色或用户等级以区分权限
                var roles = new List<string> { AppConst.User };
                // 过期时间:minutes
                var expired = 60 * 24;
                JwtService jwt = new(sign, audience, issuer)
                {
                    TokenExpires = expired,
                };
                // 添加管理员用户标识
                if (!roles.Contains(AppConst.User))
                {
                    roles.Add(AppConst.User);
                }
                var token = jwt.GetToken(user.Id.ToString(), [.. roles]);
                // 缓存登录状态
                await _cache.SetValueAsync(AppConst.LoginCachePrefix + user.Id.ToString(), true, expired * 60);

                return new AuthResult
                {
                    Id = user.Id,
                    Roles = [.. roles],
                    Token = token,
                    Username = user.UserName
                };
            }
            else
            {
                throw new Exception("缺少Jwt配置内容");
            }
        }
        else
        {
            return Problem("用户名或密码错误", title: "");
        }
    }

    /// <summary>
    /// 退出 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPut("logout/{id}")]
    public async Task<ActionResult<bool>> LogoutAsync([FromRoute] Guid id)
    {
        if (await manager.ExistAsync(id))
        {
            // 清除缓存状态
            await _cache.RemoveAsync(AppConst.LoginCachePrefix + id.ToString());
            return Ok();
        }
        return NotFound();
    }

    /// <summary>
    /// 修改密码 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPut("changePassword")]
    public async Task<ActionResult<bool>> ChangePassword(string password, string newPassword)
    {
        if (!await manager.ExistAsync(_user.UserId))
        {
            return NotFound("");
        }
        User? user = await manager.GetCurrentAsync(_user.UserId);
        return !HashCrypto.Validate(password, user!.PasswordSalt, user.PasswordHash)
            ? (ActionResult<bool>)Problem("当前密码不正确")
            : await manager.ChangePasswordAsync(user, newPassword);
    }

    /// <summary>
    /// 更新信息：头像 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<User?>> UpdateAsync(UserUpdateDto dto)
    {
        User? current = await manager.GetCurrentAsync(_user.UserId);
        if (current == null) { return NotFound(ErrorMsg.NotFoundResource); };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<User?>> GetDetailAsync()
    {
        User? res = await manager.FindAsync(_user.UserId);
        return (res == null) ? NotFound(ErrorMsg.NotFoundResource) : res;
    }
}