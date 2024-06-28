using Microsoft.AspNetCore.RateLimiting;

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
    SystemLogService logService,
    SystemRoleManager roleManager) : RestControllerBase<SystemUserManager>(manager, user, logger)
{
    private readonly SystemConfigManager _systemConfig = systemConfig;
    private readonly CacheService _cache = cache;
    private readonly IConfiguration _config = config;
    private readonly IEmailService _emailService = emailService;
    private readonly SystemLogService _logService = logService;
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
        if (!_manager.Query.Db.Any(q => q.Email != null && q.Email.Equals(email)))
        {
            return NotFound("不存在的邮箱账号");
        }
        var captcha = _manager.GetCaptcha();
        var key = AterConst.VerifyCodeCachePrefix + email;
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
        return File(_manager.GetCaptchaImage(4), "image/png");
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
        SystemUser? user = await _manager.Command.Db.Where(u => u.UserName.Equals(dto.UserName))
            .Include(u => u.SystemRoles)
            .SingleOrDefaultAsync();
        if (user == null)
        {
            return NotFound("不存在该用户");
        }

        var loginPolicy = _systemConfig.GetLoginSecurityPolicy();

        if (await _manager.ValidateLoginAsync(dto, user, loginPolicy))
        {
            // 获取Jwt配置
            JwtOption jwtOption = _config.GetSection("Authentication:Jwt").Get<JwtOption>()
                ?? throw new ArgumentNullException("未找到Jwt选项!");

            var result = _manager.GenerateJwtToken(user, jwtOption);

            var menus = new List<SystemMenu>();
            var permissionGroups = new List<SystemPermissionGroup>();
            if (user.SystemRoles != null)
            {
                menus = await _roleManager.GetSystemMenusAsync([.. user.SystemRoles]);
                permissionGroups = await _roleManager.GetPermissionGroupsAsync([.. user.SystemRoles]);
            }

            await _manager.Command.SaveChangesAsync();

            // 缓存登录状态
            string client = Request.Headers[AterConst.ClientHeader].FirstOrDefault() ?? AterConst.Web;
            if (loginPolicy.SessionLevel == SessionLevel.OnlyOne)
            {
                client = AterConst.AllPlatform;
            }

            var key = user.GetUniqueKey(AterConst.LoginCachePrefix, client);
            // 若会话过期时间为0，则使用jwt过期时间
            await _cache.SetValueAsync(key, result.Token,
                sliding: loginPolicy.SessionExpiredSeconds == 0
                ? jwtOption.ExpiredSeconds
                : loginPolicy.SessionExpiredSeconds);

            result.Menus = menus;
            result.PermissionGroups = permissionGroups;

            await _logService.NewLog("登录", UserActionType.Login, "登录成功", user.UserName, user.Id);
            return result;
        }
        else
        {
            var errorMsg = ErrorInfo.Get(_manager.ErrorStatus);
            await _logService.NewLog("登录", UserActionType.Login, "登录失败:" + errorMsg, user.UserName, user.Id);
            return Problem(errorMsg, _manager.ErrorStatus);
        }
    }
    /// <summary>
    /// 退出 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPut("logout/{id}")]
    public async Task<ActionResult<bool>> LogoutAsync([FromRoute] Guid id)
    {
        if (await _manager.ExistAsync(id))
        {
            // 清除缓存状态
            await _cache.RemoveAsync(AterConst.LoginCachePrefix + id.ToString());
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
    [Authorize(AterConst.SuperAdmin)]
    public async Task<ActionResult<PageList<SystemUserItemDto>>> FilterAsync(SystemUserFilterDto filter)
    {
        return await _manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AterConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser>> AddAsync(SystemUserAddDto dto)
    {
        SystemUser entity = await _manager.CreateNewEntityAsync(dto);
        return await _manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [Authorize(AterConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser?>> UpdateAsync([FromRoute] Guid id, SystemUserUpdateDto dto)
    {
        SystemUser? current = await _manager.GetCurrentAsync(id);
        return current == null ? NotFound(ErrorMsg.NotFoundResource) : await _manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 修改密码 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPut("changePassword")]
    public async Task<ActionResult<bool>> ChangePassword(string password, string newPassword)
    {
        if (!await _manager.ExistAsync(_user.UserId))
        {
            return NotFound("");
        }
        SystemUser? user = await _manager.GetCurrentAsync(_user.UserId);
        return !HashCrypto.Validate(password, user!.PasswordSalt, user.PasswordHash)
            ? (ActionResult<bool>)Problem("当前密码不正确")
            : (ActionResult<bool>)await _manager.ChangePasswordAsync(user, newPassword);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemUser?>> GetDetailAsync([FromRoute] Guid id)
    {
        SystemUser? res = _user.IsRole(AterConst.SuperAdmin)
            ? await _manager.FindAsync(id)
            : await _manager.FindAsync(_user.UserId);
        return res == null ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AterConst.SuperAdmin)]
    public async Task<ActionResult<SystemUser?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        SystemUser? entity = await _manager.GetCurrentAsync(id);
        return entity == null ? NotFound() : await _manager.DeleteAsync(entity, false);
    }
}