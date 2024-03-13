using SystemMod.Models.SystemMenuDtos;
namespace SystemMod.Controllers.AdminControllers;

/// <summary>
/// 系统菜单
/// </summary>
/// <see cref="SystemMenuManager"/>
[Authorize(AppConst.SuperAdmin)]
public class SystemMenuController(
    IUserContext user,
    ILogger<SystemMenuController> logger,
    SystemMenuManager manager
        ) : RestControllerBase<SystemMenuManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemMenu>>> FilterAsync(SystemMenuFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 菜单同步 ✅
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

        if (menus != null && menus.Count != 0)
        {
            return await manager.SyncSystemMenusAsync(menus);
        }
        return false;
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SystemMenu>> AddAsync(SystemMenuAddDto dto)
    {
        if (dto.ParentId != null)
        {
            if (!await manager.ExistAsync(dto.ParentId.Value))
            {
                return NotFound(ErrorMsg.NotFoundResource);
            }
        }
        SystemMenu entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<SystemMenu?>> UpdateAsync([FromRoute] Guid id, SystemMenuUpdateDto dto)
    {
        SystemMenu? current = await manager.GetCurrentAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<SystemMenu?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        SystemMenu? entity = await manager.GetCurrentAsync(id);
        if (entity == null)
        {
            return NotFound();
        };
        return await manager.DeleteAsync(entity);
    }
}