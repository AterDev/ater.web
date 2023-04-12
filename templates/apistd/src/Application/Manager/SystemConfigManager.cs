using Application.IManager;
using Share.Models.SystemConfigDtos;

namespace Application.Manager;

public class SystemConfigManager : DomainManagerBase<SystemConfig, SystemConfigUpdateDto, SystemConfigFilterDto, SystemConfigItemDto>, ISystemConfigManager
{

    private readonly IUserContext _userContext;
    public SystemConfigManager(
        DataStoreContext storeContext, 
        IUserContext userContext) : base(storeContext)
    {

        _userContext = userContext;
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<SystemConfig> CreateNewEntityAsync(SystemConfigAddDto dto)
    {
        var entity = dto.MapTo<SystemConfigAddDto, SystemConfig>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemConfig> UpdateAsync(SystemConfig entity, SystemConfigUpdateDto dto)
    {
      return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemConfigItemDto>> FilterAsync(SystemConfigFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Key, q => q.Key == filter.Key);
        // TODO: custom filter conditions
        return await Query.FilterAsync<SystemConfigItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemConfig?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
