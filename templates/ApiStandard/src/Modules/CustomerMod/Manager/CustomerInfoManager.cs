using CustomerMod.Models.CustomerInfoDtos;

namespace CustomerMod.Manager;
/// <summary>
/// 客户信息
/// </summary>
public class CustomerInfoManager(
    DataAccessContext<CustomerInfo> dataContext,
    ILogger<CustomerInfoManager> logger,
    CommandDbContext command,
    IUserContext userContext) : ManagerBase<CustomerInfo>(dataContext, logger)
{
    private readonly CommandDbContext _command = command;
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Guid?> AddAsync(CustomerInfoAddDto dto)
    {
        var entity = dto.MapTo<CustomerInfoAddDto, CustomerInfo>();
        entity.RealName = dto.Name;

        var consult = await CommandContext.SystemUsers
            .Where(q => q.Id == dto.ConsultantId)
            .FirstOrDefaultAsync();

        entity.CreatedUserId = _userContext.UserId;
        entity.ManagerId = consult!.Id;

        return await base.AddAsync(entity) ? entity.Id : null;
    }

    public async Task<bool> UpdateAsync(CustomerInfo entity, CustomerInfoUpdateDto dto)
    {
        entity.Merge(dto);
        return await base.UpdateAsync(entity);
    }

    public async Task<PageList<CustomerInfoItemDto>> ToPageAsync(CustomerInfoFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.SearchKey, q => q.Name == filter.SearchKey || q.Numbering == filter.SearchKey)
            .WhereNotNull(filter.ContactInfo, q => q.ContactInfo == filter.ContactInfo)
            .WhereNotNull(filter.CustomerType, q => q.CustomerType == filter.CustomerType)
            .WhereNotNull(filter.FollowUpStatus, q => q.FollowUpStatus == filter.FollowUpStatus)
            .WhereNotNull(filter.GenderType, q => q.GenderType == filter.GenderType);

        return await base.ToPageAsync<CustomerInfoFilterDto, CustomerInfoItemDto>(filter);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CustomerInfo?> GetDetailAsync(Guid id)
    {
        return await Queryable
            .Where(q => q.Id == id)
            .Include(q => q.Tags)
            .Include(q => q.CustomerRegister)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 是否唯一
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsConflictAsync(string name, string contactInfo)
    {
        // 自定义唯一性验证参数和逻辑
        return await Command.AnyAsync(q => q.Name.Equals(name) && q.ContactInfo!.Equals(contactInfo));
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CustomerInfo?> GetOwnedAsync(Guid id)
    {
        var query = Command.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
