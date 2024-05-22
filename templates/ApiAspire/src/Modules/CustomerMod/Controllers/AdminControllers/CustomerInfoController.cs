using CustomerMod.Models.CustomerInfoDtos;
namespace CustomerMod.Controllers.AdminControllers;

/// <summary>
/// 客户信息
/// </summary>
/// <see cref="CustomerMod.Manager.CustomerInfoManager"/>
public class CustomerInfoController(
    IUserContext user,
    ILogger<CustomerInfoController> logger,
    CustomerInfoManager manager
    ) : RestControllerBase<CustomerInfoManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<CustomerInfoItemDto>>> FilterAsync(CustomerInfoFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CustomerInfo>> AddAsync(CustomerInfoAddDto dto)
    {
        if (await manager.IsConflictAsync(dto.Name, dto.ContactInfo))
        {
            return Conflict(ErrorMsg.ConflictResource);
        }
        var entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 部分更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<CustomerInfo?>> UpdateAsync([FromRoute] Guid id, CustomerInfoUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null) { return NotFound("不存在的资源"); };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerInfo?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.GetDetailAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomerInfo?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }
}