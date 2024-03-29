using Microsoft.AspNetCore.RateLimiting;

using Share.Models.UserDtos;

using SystemMod.Models;
using SystemMod.Models.SystemUserDtos;

namespace SystemMod.Controllers.AdminControllers;

/// <summary>
/// 系统用户
/// </summary>
public class SystemUserController(
    IUserContext user,
    ILogger<SystemUserController> logger,
    SystemUserManager manager,
    SystemConfigManager systemConfig,
    CacheService cache,
    IConfiguration config,
    IEmailService emailService,
    SystemRoleManager roleManager) : RestControllerBase<SystemUserManager>(manager, user, logger)
{
    private readonly SystemConfigManager _systemConfig = systemConfig;
    private readonly CacheService _cache = cache;
    private readonly IConfiguration _config = config;
    private readonly IEmailService _emailService = emailService;
    private readonly SystemRoleManager _roleManager = roleManager;

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
    /// 获取图形验证码 ✅
    /// </summary>
    /// <returns></returns>
    [HttpGet("captcha")]
    [EnableRateLimiting("captcha")]
    [AllowAnonymous]
    public ActionResult GetCaptchaImage()
    {
        return File(manager.GetCaptchaImage(4), "image/png");
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
        dto.Password = dto.Password.Trim();
        // 查询用户
        SystemUser? user = await manager.Command.Db.Where(u => u.UserName.Equals(dto.UserName))
            .AsNoTracking()
            .Include(u => u.SystemRoles)
            .SingleOrDefaultAsync();
        if (user == null)
        {
            return NotFound("不存在该用户");
        }

        var loginPolicy = _systemConfig.GetLoginSecurityPolicy();

        if (await manager.ValidateLoginAsync(dto, user, loginPolicy))
        {
            // 获取Jwt配置
            JwtOption jwtOption = _config.GetSection("Authentication:Jwt").Get<JwtOption>()
                ?? throw new ArgumentNullException("未找到Jwt选项!");

            var result = manager.GenerateJwtToken(user, jwtOption);

            var menus = new List<SystemMenu>();
            var permissionGroups = new List<SystemPermissionGroup>();
            if (user.SystemRoles != null)
            {
                menus = await _roleManager.GetSystemMenusAsync([.. user.SystemRoles]);
                permissionGroups = await _roleManager.GetPermissionGroupsAsync([.. user.SystemRoles]);
            }
            await manager.Command.SaveChangesAsync();

            // 缓存登录状态
            string client = Request.Headers[AppConst.ClientHeader].FirstOrDefault() ?? AppConst.Web;
            if (loginPolicy.SessionLevel == SessionLevel.None)
            {
                client = AppConst.AllPlatform;
            }

            await _cache.SetValueAsync(user.GetUniqueKey(AppConst.LoginCachePrefix, client), result.Token, sliding: loginPolicy.SessionExpiredSeconds);

            result.Menus = menus;
            result.PermissionGroups = permissionGroups;
            return result;
        }
        else
        {
            return Problem(manager.ErrorMsg);
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
        SystemUser entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [Authorize(AppConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser?>> UpdateAsync([FromRoute] Guid id, SystemUserUpdateDto dto)
    {
        SystemUser? current = await manager.GetCurrentAsync(id);
        return current == null ? (ActionResult<SystemUser?>)NotFound(ErrorMsg.NotFoundResource) : (ActionResult<SystemUser?>)await manager.UpdateAsync(current, dto);
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
        SystemUser? user = await manager.GetCurrentAsync(_user.UserId);
        return !HashCrypto.Validate(password, user!.PasswordSalt, user.PasswordHash)
            ? (ActionResult<bool>)Problem("当前密码不正确")
            : (ActionResult<bool>)await manager.ChangePasswordAsync(user, newPassword);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemUser?>> GetDetailAsync([FromRoute] Guid id)
    {
        SystemUser? res = _user.IsRole(AppConst.SuperAdmin)
            ? await manager.FindAsync(id)
            : await manager.FindAsync(_user.UserId);
        return res == null ? NotFound() : res;
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
        SystemUser? entity = await manager.GetCurrentAsync(id);
        return entity == null ? (ActionResult<SystemUser?>)NotFound() : (ActionResult<SystemUser?>)await manager.DeleteAsync(entity);
    }
}