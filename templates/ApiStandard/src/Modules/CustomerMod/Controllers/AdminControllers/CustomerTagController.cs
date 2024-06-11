using CustomerMod.Models.CustomerTagDtos;
namespace CustomerMod.Controllers.AdminControllers;

/// <summary>
/// 用户标签
/// </summary>
/// <see cref="CustomerMod.Manager.CustomerTagManager"/>
public class CustomerTagController(
    IUserContext user,
    ILogger<CustomerTagController> logger,
    CustomerTagManager manager
    ) : RestControllerBase<CustomerTagManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<CustomerTagItemDto>>> FilterAsync(CustomerTagFilterDto filter)
    {
        return await _manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CustomerTag>> AddAsync(CustomerTagAddDto dto)
    {
        var entity = await _manager.CreateNewEntityAsync(dto);
        return await _manager.AddAsync(entity);
    }

    /// <summary>
    /// 部分更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<CustomerTag?>> UpdateAsync([FromRoute] Guid id, CustomerTagUpdateDto dto)
    {
        var current = await _manager.GetCurrentAsync(id);
        if (current == null) { return NotFound("不存在的资源"); };
        return await _manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerTag?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await _manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomerTag?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await _manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await _manager.DeleteAsync(entity);
    }
}