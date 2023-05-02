using Share.Models.SystemMenuDtos;

namespace Application.Manager;

public class SystemMenuManager : DomainManagerBase<SystemMenu, SystemMenuUpdateDto, SystemMenuFilterDto, SystemMenuItemDto>, ISystemMenuManager
{
    public SystemMenuManager(
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
    public Task<SystemMenu> CreateNewEntityAsync(SystemMenuAddDto dto)
    {
        SystemMenu entity = dto.MapTo<SystemMenuAddDto, SystemMenu>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemMenu> UpdateAsync(SystemMenu entity, SystemMenuUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemMenuItemDto>> FilterAsync(SystemMenuFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.AccessCode, q => q.AccessCode == filter.AccessCode);
        // TODO: custom filter conditions
        return await Query.FilterAsync<SystemMenuItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemMenu?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemMenu> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
