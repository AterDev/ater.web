using CustomerMod.Models.CustomerInfoDtos;

namespace CustomerMod.Manager;
/// <summary>
/// 客户信息
/// </summary>
public class CustomerInfoManager(
    DataAccessContext<CustomerInfo> dataContext,
    ILogger<CustomerInfoManager> logger,
    CommandDbContext command,
    IUserContext userContext) : ManagerBase<CustomerInfo, CustomerInfoUpdateDto, CustomerInfoFilterDto, CustomerInfoItemDto>(dataContext, logger)
{
    private readonly CommandDbContext _command = command;
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<CustomerInfo> CreateNewEntityAsync(CustomerInfoAddDto dto)
    {
        var entity = dto.MapTo<CustomerInfoAddDto, CustomerInfo>();
        entity.RealName = dto.Name;

        var consult = await CommandContext.SystemUsers
            .Where(q => q.Id == dto.ConsultantId)
            .FirstOrDefaultAsync();

        entity.CreatedUserId = _userContext.UserId;
        entity.ManagerId = consult!.Id;

        return await Task.FromResult(entity);
    }

    public override async Task<CustomerInfo> UpdateAsync(CustomerInfo entity, CustomerInfoUpdateDto dto)
    {
        /*
        if (dto.CustomerTagIds != null && dto.CustomerTagIds.Count > 0)
        {
            var tags = await CommandContext.Tags()
                .Where(t => dto.CustomerTagIds.Contains(t.Id))
                .ToListAsync();
            if (tags != null)
            {
                entity.Tags = tags;
            }
        }
        */
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<CustomerInfoItemDto>> FilterAsync(CustomerInfoFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.SearchKey, q => q.Name == filter.SearchKey || q.Numbering == filter.SearchKey)
            .WhereNotNull(filter.ContactInfo, q => q.ContactInfo == filter.ContactInfo)
            .WhereNotNull(filter.CustomerType, q => q.CustomerType == filter.CustomerType)
            .WhereNotNull(filter.FollowUpStatus, q => q.FollowUpStatus == filter.FollowUpStatus)
            .WhereNotNull(filter.GenderType, q => q.GenderType == filter.GenderType);

        return await Query.FilterAsync<CustomerInfoItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
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
        return await Command.Db.AnyAsync(q => q.Name.Equals(name) && q.ContactInfo!.Equals(contactInfo));
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CustomerInfo?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
