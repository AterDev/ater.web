using Share.Models.SystemRoleDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统角色
/// <see cref="Application.Manager.SystemRoleManager"/>
/// </summary>
[Authorize(AppConst.SuperAdmin)]
public class SystemRoleController : RestControllerBase<SystemRoleManager>
{

    public SystemRoleController(
        IUserContext user,
        ILogger<SystemRoleController> logger,
        SystemRoleManager manager
        ) : base(manager, user, logger)
    {

    }

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemRoleItemDto>>> FilterAsync(SystemRoleFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SystemRole>> AddAsync(SystemRoleAddDto dto)
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
    public async Task<ActionResult<SystemRole?>> UpdateAsync([FromRoute] Guid id, SystemRoleUpdateDto dto)
    {
        var current = await manager.GetOwnedAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        }

        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 角色菜单 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("menus")]
    public async Task<ActionResult<SystemRole?>> UpdateMenusAsync([FromBody] SystemRoleSetMenusDto dto)
    {
        var current = await manager.GetCurrentAsync(dto.Id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        }
        var res = await manager.SetMenusAsync(current, dto);
        return Ok(res) ?? Problem("菜单更新失败");
    }

    /// <summary>
    /// Set Permission Group ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("permissionGroups")]
    public async Task<ActionResult<SystemRole?>> UpdatePermissionGroupsAsync([FromBody] SystemRoleSetPermissionGroupsDto dto)
    {
        var current = await manager.GetCurrentAsync(dto.Id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        }
        var res = await manager.SetPermissionGroupsAsync(current, dto);
        return Ok(res) ?? Problem("权限组更新失败");
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemRole?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<SystemRole?>> DeleteAsync([FromRoute] Guid id)
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