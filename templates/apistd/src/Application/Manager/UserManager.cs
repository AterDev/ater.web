using Share.Models.UserDtos;

namespace Application.Manager;

public class UserManager : DomainManagerBase<User, UserUpdateDto, UserFilterDto, UserItemDto>, IUserManager
{

    private readonly IUserContext _userContext;
    public UserManager(
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
    public Task<User> CreateNewEntityAsync(UserAddDto dto)
    {
        User entity = dto.MapTo<UserAddDto, User>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<User> UpdateAsync(User entity, UserUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<UserItemDto>> FilterAsync(UserFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.UserName, q => q.UserName == filter.UserName);
        // TODO: custom filter conditions
        return await Query.FilterAsync<UserItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> GetOwnedAsync(Guid id)
    {
        IQueryable<User> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
