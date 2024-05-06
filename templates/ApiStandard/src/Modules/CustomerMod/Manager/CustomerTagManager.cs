using CustomerMod.Models.CustomerTagDtos;

namespace CustomerMod.Manager;
/// <summary>
/// 用户标签
/// </summary>
public class CustomerTagManager(
    DataAccessContext<CustomerTag> dataContext,
    ILogger<CustomerTagManager> logger,
    IUserContext userContext) : ManagerBase<CustomerTag, CustomerTagUpdateDto, CustomerTagFilterDto, CustomerTagItemDto>(dataContext, logger)
{
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<CustomerTag> CreateNewEntityAsync(CustomerTagAddDto dto)
    {
        var entity = dto.MapTo<CustomerTagAddDto, CustomerTag>();
        /*
        if (dto.CustomerInfoIds != null && dto.CustomerInfoIds.Count > 0)
        {
            var customers = await CommandContext.Customers()
                .Where(t => dto.CustomerInfoIds.Contains(t.Id))
                .ToListAsync();
            if (customers != null)
            {
                entity.Customers = customers;
            }
        }
        */
        // other required props
        return await Task.FromResult(entity);
    }

    public override async Task<CustomerTag> UpdateAsync(CustomerTag entity, CustomerTagUpdateDto dto)
    {
        /*
        if (dto.CustomerInfoIds != null && dto.CustomerInfoIds.Count > 0)
        {
            var customers = await CommandContext.Customers()
                .Where(t => dto.CustomerInfoIds.Contains(t.Id))
                .ToListAsync();
            if (customers != null)
            {
                entity.Customers = customers;
            }
        }
        */
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<CustomerTagItemDto>> FilterAsync(CustomerTagFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.DisplayName, q => q.DisplayName == filter.DisplayName)
            .WhereNotNull(filter.Key, q => q.Key == filter.Key);
        // TODO: custom filter conditions
        return await Query.FilterAsync<CustomerTagItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }


    /// <summary>
    /// 是否唯一
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsConflictAsync(string unique)
    {
        // TODO:自定义唯一性验证参数和逻辑
        return await Command.Db.AnyAsync(q => q.Id == new Guid(unique));
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CustomerTag?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
