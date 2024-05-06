using CustomerMod.Models.CustomerRegisterDtos;

namespace CustomerMod.Manager;
/// <summary>
/// 客户登记
/// </summary>
public class CustomerRegisterManager(
    DataAccessContext<CustomerRegister> dataContext,
    ILogger<CustomerRegisterManager> logger,
    CacheService cache,
    IUserContext userContext) : ManagerBase<CustomerRegister, CustomerRegisterUpdateDto, CustomerRegisterFilterDto, CustomerRegisterItemDto>(dataContext, logger)
{
    private readonly CacheService _cache = cache;
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<CustomerRegister> CreateNewEntityAsync(CustomerRegisterAddDto dto)
    {
        var entity = dto.MapTo<CustomerRegisterAddDto, CustomerRegister>();
        return await Task.FromResult(entity);
    }

    public override async Task<CustomerRegister> AddAsync(CustomerRegister entity)
    {
        Command.Database.BeginTransaction();
        try
        {
            await base.AddAsync(entity);
            var customer = await CommandContext.CustomerInfos.Where(q => q.ContactInfo == entity.Weixin).FirstOrDefaultAsync();

            if (customer != null)
            {
                customer.CustomerRegisterId = entity.Id;
            }
            await Command.SaveChangesAsync();
            Command.Database.CommitTransaction();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登记失败");
            Command.Database.RollbackTransaction();
        }
        return entity;
    }

    public override async Task<CustomerRegister> UpdateAsync(CustomerRegister entity, CustomerRegisterUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<CustomerRegisterItemDto>> FilterAsync(CustomerRegisterFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.PhoneNumber, q => q.PhoneNumber == filter.PhoneNumber)
            .WhereNotNull(filter.Weixin, q => q.Weixin == filter.Weixin);
        return await Query.FilterAsync<CustomerRegisterItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }


    /// <summary>
    /// 是否唯一
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsConflictAsync(string weixin, string phone)
    {
        // TODO:自定义唯一性验证参数和逻辑
        return await Command.Db.AnyAsync(q => q.Weixin.Equals(weixin) || q.PhoneNumber.Equals(phone));
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CustomerRegister?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// 验证临时码
    /// </summary>
    /// <returns></returns>
    public bool VerifyTempCode(string code)
    {
        var res = _cache.GetValue<bool?>(Constant.Cache.RegisterPrefix + code);
        if (res == null || !res.Value)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 获取临时码
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetTempCodeAsync()
    {
        var code = HashCrypto.GetRnd(8, useLow: true);
        await _cache.SetValueAsync(Constant.Cache.RegisterPrefix + code, true, 60 * 60 * 2);
        return code;
    }

    /// <summary>
    /// 清除验证码
    /// </summary>
    /// <param name="code"></param>
    public async Task ClearTempCodeAsync(string code)
    {
        await _cache.RemoveAsync(Constant.Cache.RegisterPrefix + code);
    }
}
