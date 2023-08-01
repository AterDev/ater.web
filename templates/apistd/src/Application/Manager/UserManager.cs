using Application.Implement;
using Application.IManager;
using Share.Models.UserDtos;

namespace Application.Manager;
/// <summary>
/// 用户账户
/// </summary>
public class UserManager : DomainManagerBase<User, UserUpdateDto, UserFilterDto, UserItemDto>, IUserManager
{

    public UserManager(
        DataStoreContext storeContext, 
        ILogger<UserManager> logger,
        IUserContext userContext) : base(storeContext, logger)
    {

        _userContext = userContext;
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<User> CreateNewEntityAsync(UserAddDto dto)
    {
        var entity = dto.MapTo<UserAddDto, User>();
        // other required props
        return await Task.FromResult(entity);
    }

    public override async Task<User> UpdateAsync(User entity, UserUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<UserItemDto>> FilterAsync(UserFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.UserName, q => q.UserName == filter.UserName)
            .WhereNotNull(filter.UserType, q => q.UserType == filter.UserType)
            .WhereNotNull(filter.EmailConfirmed, q => q.EmailConfirmed == filter.EmailConfirmed)
            .WhereNotNull(filter.PhoneNumberConfirmed, q => q.PhoneNumberConfirmed == filter.PhoneNumberConfirmed)
            .WhereNotNull(filter.TwoFactorEnabled, q => q.TwoFactorEnabled == filter.TwoFactorEnabled)
            .WhereNotNull(filter.LockoutEnabled, q => q.LockoutEnabled == filter.LockoutEnabled);
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
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
