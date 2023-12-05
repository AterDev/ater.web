using FileManagerMod.Models.FolderDtos;
namespace FileManagerMod.Controllers;

/// <summary>
/// 文件夹
/// </summary>
/// <see cref="FileManagerMod.Manager.FolderManager"/>
public class FolderController : ClientControllerBase<FolderManager>
{

    public FolderController(
        IUserContext user,
        ILogger<FolderController> logger,
        FolderManager manager
        ) : base(manager, user, logger)
    {

    }

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<FolderItemDto>>> FilterAsync(FolderFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Folder>> AddAsync(FolderAddDto dto)
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
    public async Task<ActionResult<Folder?>> UpdateAsync([FromRoute] Guid id, FolderUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null) { return NotFound(ErrorMsg.NotFoundResource); };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Folder?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// 删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Folder?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }
}