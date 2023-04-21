using Share.Models.UserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface IUserManager :IDomainManager<User>{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<User> CreateNewEntityAsync(UserAddDto dto);
    Task<User?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<User> AddAsync(User entity);
    Task<User> UpdateAsync(User entity, UserUpdateDto dto);
    Task<User?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<User, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<User, bool>>? whereExp) where TDto : class;
    Task<PageList<UserItemDto>> FilterAsync(UserFilterDto filter);
    Task<User?> DeleteAsync(User entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
