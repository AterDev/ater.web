using Application;
using Entity.FileManagerMod;
using FileManagerMod.Models.FolderDtos;

namespace FileManagerMod.Controllers.AdminControllers;

/// <summary>
/// 文件夹
/// </summary>
/// <see cref="FileManagerMod.Manager.FolderManager"/>
public class FolderController(
    IUserContext user,
    ILogger<FolderController> logger,
    FolderManager manager
        ) : RestControllerBase<FolderManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<FolderItemDto>>> FilterAsync(FolderFilterDto filter)
    {
        return await _manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Folder>> AddAsync(FolderAddDto dto)
    {
        if (dto.ParentId != null)
        {
            var exist = await _manager.ExistAsync(dto.ParentId.Value);
            if (!exist)
            {
                return NotFound(ErrorMsg.NotFoundResource);
            };
        }
        Folder entity = await _manager.CreateNewEntityAsync(dto);
        return await _manager.AddAsync(entity);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Folder?>> GetDetailAsync([FromRoute] Guid id)
    {
        Folder? res = await _manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Folder?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        Folder? entity = await _manager.GetCurrentAsync(id);
        if (entity == null)
        {
            return NotFound();
        };
        // return Forbid();
        return await _manager.DeleteAsync(entity, false);
    }
}