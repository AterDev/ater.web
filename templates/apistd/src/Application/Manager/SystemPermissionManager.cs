using Share.Models.SystemPermissionDtos;

namespace Application.Manager;

public class SystemPermissionManager : DomainManagerBase<SystemPermission, SystemPermissionUpdateDto, SystemPermissionFilterDto, SystemPermissionItemDto>, ISystemPermissionManager
{
    public SystemPermissionManager(
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
    public Task<SystemPermission> CreateNewEntityAsync(SystemPermissionAddDto dto)
    {
        SystemPermission entity = dto.MapTo<SystemPermissionAddDto, SystemPermission>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemPermission> UpdateAsync(SystemPermission entity, SystemPermissionUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemPermissionItemDto>> FilterAsync(SystemPermissionFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name);
        // TODO: custom filter conditions
        return await Query.FilterAsync<SystemPermissionItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemPermission?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemPermission> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
