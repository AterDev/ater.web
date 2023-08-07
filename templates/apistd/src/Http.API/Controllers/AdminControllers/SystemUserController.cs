using Share.Models.AuthDtos;
using Share.Models.SystemUserDtos;

namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统用户
/// </summary>
public class SystemUserController : RestControllerBase<ISystemUserManager>
{
    private readonly CacheService _cache;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;

    public SystemUserController(
        IUserContext user,
        ILogger<SystemUserController> logger,
        ISystemUserManager manager,
        CacheService cache,
        IConfiguration config,
        IEmailService emailService) : base(manager, user, logger)
    {
        _cache = cache;
        _config = config;
        _emailService = emailService;
    }

    /// <summary>
    /// 登录时，发送邮箱验证码 ✅
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("verifyCode")]
    [AllowAnonymous]
    public async Task<ActionResult> SendVerifyCodeAsync(string email)
    {
        if (!manager.Query.Db.Any(q => q.Email != null && q.Email.Equals(email)))
        {
            return NotFound("不存在的邮箱账号");
        }
        var captcha = manager.GetCaptcha();
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
        var user = await manager.Command.Db.Where(u => u.UserName.Equals(dto.UserName))
            .Include(u => u.SystemRoles)
            .SingleOrDefaultAsync();
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
                return NotFound("验证码已过期");
            }
            if (!cacheCode.Equals(dto.VerifyCode))
            {
                return NotFound("验证码错误");
            }
        }
        if (HashCrypto.Validate(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            // 获取Jwt配置
            var jwtOption = _config.GetSection("Authentication:Jwt").Get<JwtOption>()
                ?? throw new ArgumentNullException("未找到Jwt选项!");
            var sign = jwtOption.Sign;
            var issuer = jwtOption.ValidIssuer;
            var audience = jwtOption.ValidAudiences;

            // 构建返回内容
            if (!string.IsNullOrWhiteSpace(sign) &&
                !string.IsNullOrWhiteSpace(issuer) &&
                !string.IsNullOrWhiteSpace(audience))
            {
                // 加载关联数据
                manager.LoadRolesWithPermissions(user);

                var roles = user.SystemRoles?.Select(r => r.NameValue)?.ToList()
                    ?? new List<string> { AppConst.AdminUser };
                // 过期时间:minutes
                var expired = 60 * 24;
                JwtService jwt = new(sign, audience, issuer)
                {
                    TokenExpires = expired,
                };
                // 添加管理员用户标识
                if (!roles.Contains(AppConst.AdminUser))
                {
                    roles.Add(AppConst.AdminUser);
                }
                var token = jwt.GetToken(user.Id.ToString(), roles.ToArray());
                // 缓存登录状态
                await _cache.SetValueAsync(AppConst.LoginCachePrefix + user.Id.ToString(), true, expired * 60);

                var menus = user.SystemRoles?.SelectMany(r => r.Menus).ToList();
                var permissionGroups = user.SystemRoles?.SelectMany(r => r.PermissionGroups).ToList();

                // 记录登录时间
                user.LastLoginTime = DateTimeOffset.UtcNow;
                await manager.Command.SaveChangeAsync();

                return new AuthResult
                {
                    Id = user.Id,
                    Roles = roles.ToArray(),
                    Token = token,
                    Menus = menus,
                    PermissionGroups = permissionGroups,
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
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    [Authorize(AppConst.SuperAdmin)]
    public async Task<ActionResult<PageList<SystemUserItemDto>>> FilterAsync(SystemUserFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AppConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser>> AddAsync(SystemUserAddDto dto)
    {
        var entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(AppConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser?>> UpdateAsync([FromRoute] Guid id, SystemUserUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        }

        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 修改密码 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPut("changePassword")]
    public async Task<ActionResult<bool>> ChangePassword(string password, string newPassword)
    {
        if (!await manager.ExistAsync(_user.UserId!.Value))
        {
            return NotFound("");
        }
        var user = await manager.GetCurrentAsync(_user.UserId!.Value);
        if (!HashCrypto.Validate(password, user!.PasswordSalt, user.PasswordHash))
        {
            return Problem("当前密码不正确");
        }
        return await manager.ChangePasswordAsync(user, newPassword);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemUser?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = _user.IsRole(AppConst.SuperAdmin)
            ? await manager.FindAsync(id)
            : await manager.FindAsync(_user.UserId!.Value);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AppConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetCurrentAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        return await manager.DeleteAsync(entity);
    }
}