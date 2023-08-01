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
    public SystemUserController(
        IUserContext user,
        ILogger<SystemUserController> logger,
        ISystemUserManager manager,
        CacheService cache,
        IConfiguration config) : base(manager, user, logger)
    {
        _cache = cache;
        _config = config;
    }

    /// <summary>
    /// 登录时，发送邮箱验证码
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
        var key = "VerifyCode:" + email;
        if (_cache.GetValue<string>(key) != null)
        {
            return Conflict("验证码已发送!");
        }
        // 缓存，默认60秒过期
        await _cache.SetValueAsync(key, captcha, 60);
        // 自定义html内容
        var htmlContent = $"登录验证码:{captcha}";
        // 使用 smtp，可替换成其他
        await manager.SendVerifyEmailAsync(email, "登录验证码", htmlContent);
        return Ok();
    }

    /// <summary>
    /// 登录获取Token
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResult>> LoginAsync(LoginDto dto)
    {
        // 查询用户
        var user = await manager.Query.Db.Where(u => u.UserName.Equals(dto.UserName))
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("不存在该用户");
        }

        if (HashCrypto.Validate(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            // 获取Jwt配置
            var jwtOption = _config.GetSection("Authentication:Jwt").Get<JwtOption>()
                ?? throw new ArgumentNullException("未找到Jwt选项!");
            var sign = jwtOption.Sign;
            var issuer = jwtOption.ValidIssuer;
            var audience = jwtOption.ValidAudiences;

            //var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //1天后过期
            if (!string.IsNullOrWhiteSpace(sign) &&
                !string.IsNullOrWhiteSpace(issuer) &&
                !string.IsNullOrWhiteSpace(audience))
            {
                manager.LoadRolesWithPermissions(user);
                var roles = user.SystemRoles?.Select(r => r.NameValue)?.ToList()
                    ?? new List<string> { AppConst.AdminUser };

                JwtService jwt = new(sign, audience, issuer)
                {
                    TokenExpires = 60 * 24 * 7,
                };
                // 添加管理员用户标识
                if (!roles.Contains(AppConst.AdminUser))
                {
                    roles.Add(AppConst.AdminUser);
                }
                var token = jwt.GetToken(user.Id.ToString(), roles.ToArray());
                // 登录状态存储到Redis
                //await _redis.SetValueAsync("login" + user.Id.ToString(), true, 60 * 24 * 7);

                var menus = user.SystemRoles?.SelectMany(r => r.Menus).ToList();
                var permissionGroups = user.SystemRoles?.SelectMany(r => r.PermissionGroups).ToList();
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
    /// 退出 
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> LogoutAsync([FromRoute] Guid id)
    {
        //var user = await _store.FindAsync(id);
        //if (user == null) return NotFound();
        // 清除redis登录状态
        //await _redis.Cache.RemoveAsync(id.ToString());
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemUserItemDto>>> FilterAsync(SystemUserFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SystemUser>> AddAsync(SystemUserAddDto dto)
    {
        var entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<SystemUser?>> UpdateAsync([FromRoute] Guid id, SystemUserUpdateDto dto)
    {
        var current = await manager.GetOwnedAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        }

        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemUser?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<SystemUser?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetOwnedAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }
}