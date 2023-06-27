using Share.Models.SystemRoleDtos;

namespace Application.Manager;

public class SystemRoleManager : DomainManagerBase<SystemRole, SystemRoleUpdateDto, SystemRoleFilterDto, SystemRoleItemDto>, ISystemRoleManager
{
    public SystemRoleManager(
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
    public Task<SystemRole> CreateNewEntityAsync(SystemRoleAddDto dto)
    {
        SystemRole entity = dto.MapTo<SystemRoleAddDto, SystemRole>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemRole> UpdateAsync(SystemRole entity, SystemRoleUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemRoleItemDto>> FilterAsync(SystemRoleFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.NameValue, q => q.NameValue == filter.NameValue);
        // TODO: custom filter conditions
        return await Query.FilterAsync<SystemRoleItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemRole?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemRole> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
