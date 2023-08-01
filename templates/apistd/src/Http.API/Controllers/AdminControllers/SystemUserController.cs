using Share.Models.SystemUserDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统用户
/// </summary>
public class SystemUserController : RestControllerBase<ISystemUserManager>
{
    private readonly CacheService _cache;
    public SystemUserController(
        IUserContext user,
        ILogger<SystemUserController> logger,
        ISystemUserManager manager,
        CacheService cache) : base(manager, user, logger)
    {
        _cache = cache;
    }

    [HttpPost("verifyCode")]
    public async Task<ActionResult> SendVerifyCodeAsync(string targetAddress)
    {
        var captcha = manager.GetCaptcha();
        var key = "VerifyCode:" + targetAddress;
        if (_cache.GetValue<string>(key) != null)
        {
            return Conflict("验证码已发送!");
        }
        // 缓存，默认60过期
        await _cache.SetValueAsync(key, captcha, 60);

        // 使用 smtp，可替换成其他
        SmtpService.Create()
            .SetCredentials()
            .SendEmailAsync(targetAddress, "验证码", captcha);
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