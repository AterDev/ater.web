using Share.Models.SystemUserDtos;

namespace Application.Manager;

public class SystemUserManager : DomainManagerBase<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>, ISystemUserManager
{

    private readonly IUserContext _userContext;
    public SystemUserManager(
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
    public Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto)
    {
        SystemUser entity = dto.MapTo<SystemUserAddDto, SystemUser>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemUser> UpdateAsync(SystemUser entity, SystemUserUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemUserItemDto>> FilterAsync(SystemUserFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.UserName, q => q.UserName == filter.UserName);
        // TODO: custom filter conditions
        return await Query.FilterAsync<SystemUserItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemUser?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemUser> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
