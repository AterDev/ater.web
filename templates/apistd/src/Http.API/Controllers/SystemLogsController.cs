using Share.Models.SystemLogsDtos;
namespace Http.API.Controllers;

/// <summary>
/// 系统日志
/// </summary>
/// <see cref="Application.Manager.SystemLogsManager"/>
public class SystemLogsController : ClientControllerBase<ISystemLogsManager>
{
    private readonly ISystemUserManager _systemUserManager;

    public SystemLogsController(
        IUserContext user,
        ILogger<SystemLogsController> logger,
        ISystemLogsManager manager,
        ISystemUserManager systemUserManager
        ) : base(manager, user, logger)
    {
        _systemUserManager = systemUserManager;

    }

    /// <summary>
    /// Operation Log Query
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemLogsItemDto>>> FilterAsync(SystemLogsFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }
}