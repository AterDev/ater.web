using Share.Models.SystemLogsDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统日志
/// </summary>
/// <see cref="Application.Manager.SystemLogsManager"/>
public class SystemLogsController : RestControllerBase<SystemLogsManager>
{
    private readonly SystemUserManager _systemUserManager;

    public SystemLogsController(
        IUserContext user,
        ILogger<SystemLogsController> logger,
        SystemLogsManager manager,
        SystemUserManager systemUserManager
        ) : base(manager, user, logger)
    {
        _systemUserManager = systemUserManager;

    }

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemLogsItemDto>>> FilterAsync(SystemLogsFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

}