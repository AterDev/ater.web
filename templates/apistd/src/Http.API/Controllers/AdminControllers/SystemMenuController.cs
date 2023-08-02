using Share.Models.SystemMenuDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统菜单
/// </summary>
/// <see cref="Application.Manager.SystemMenuManager"/>
public class SystemMenuController : RestControllerBase<ISystemMenuManager>
{

    public SystemMenuController(
        IUserContext user,
        ILogger<SystemMenuController> logger,
        ISystemMenuManager manager
        ) : base(manager, user, logger)
    {

    }

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemMenuItemDto>>> FilterAsync(SystemMenuFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 同步菜单
    /// </summary>
    /// <param name="token"></param>
    /// <param name="menus"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("sync/{token}")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> SyncSystemMenus(string token, List<SystemMenuSyncDto> menus)
    {
        // 不经过jwt验证，定义自己的key用来开发时同步菜单
        if (token != "MyProjectNameDefaultKey")
        {
            return Forbid();
        }

        if (menus != null && menus.Any())
        {
            return await manager.SyncSystemMenusAsync(menus);
        }
        return false;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SystemMenu>> AddAsync(SystemMenuAddDto dto)
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
    public async Task<ActionResult<SystemMenu?>> UpdateAsync([FromRoute] Guid id, SystemMenuUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null) { return NotFound(ErrorMsg.NotFoundResource); };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemMenu?>> GetDetailAsync([FromRoute] Guid id)
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
    public async Task<ActionResult<SystemMenu?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }
}