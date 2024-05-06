using CustomerMod.Models.CustomerRegisterDtos;
namespace CustomerMod.Controllers.AdminControllers;

/// <summary>
/// 客户登记
/// </summary>
/// <see cref="CustomerMod.Manager.CustomerRegisterManager"/>
public class CustomerRegisterController(

    IUserContext user,
    ILogger<CustomerRegisterController> logger,
    CustomerRegisterManager manager
    ) : RestControllerBase<CustomerRegisterManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<CustomerRegisterItemDto>>> FilterAsync(CustomerRegisterFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CustomerRegister>> AddAsync(CustomerRegisterAddDto dto)
    {
        if (await manager.IsConflictAsync(dto.Weixin, dto.PhoneNumber))
        {
            return Conflict(ErrorMsg.ConflictResource);
        }

        var entity = await manager.CreateNewEntityAsync(dto);
        var res = await manager.AddAsync(entity);
        await manager.ClearTempCodeAsync(dto.VerifyCode);
        return res;
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerRegister?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<CustomerRegister?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        var entity = await manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }

    /// <summary>
    /// 获取临时码
    /// </summary>
    /// <returns></returns>
    [HttpGet("temp_code")]
    public async Task<ActionResult<string>> GetTempCodeAsync()
    {
        return await manager.GetTempCodeAsync();
    }

    /// <summary>
    /// 验证临时码
    /// </summary>
    /// <returns></returns>
    [HttpGet("verify_temp_code")]
    [AllowAnonymous]
    public ActionResult<bool> VerifyTempCode(string code)
    {
        return manager.VerifyTempCode(code);
    }
}