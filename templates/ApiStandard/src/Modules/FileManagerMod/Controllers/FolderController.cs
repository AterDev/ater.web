using Application;
using FileManagerMod.Models.FolderDtos;
namespace FileManagerMod.Controllers;

/// <summary>
/// 文件夹
/// </summary>
/// <see cref="FileManagerMod.Manager.FolderManager"/>
public class FolderController(
    IUserContext user,
    ILogger<FolderController> logger,
    FolderManager manager
        ) : ClientControllerBase<FolderManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<FolderItemDto>>> FilterAsync(FolderFilterDto filter)
    {
        return await _manager.ToPageAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Guid?>> AddAsync(FolderAddDto dto)
    {
        var id = await _manager.AddAsync(dto);
        return id == null ? Problem(Constant.AddFailed) : id;
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<bool?>> UpdateAsync([FromRoute] Guid id, FolderUpdateDto dto)
    {
        Folder? current = await _manager.GetCurrentAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        };
        return await _manager.UpdateAsync(current, dto);
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
    /// 删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        Folder? entity = await _manager.GetCurrentAsync(id);
        return entity == null ? NotFound() : await _manager.DeleteAsync([id], false);
    }
}