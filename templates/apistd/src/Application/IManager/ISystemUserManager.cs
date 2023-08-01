using Share.Models.SystemUserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemUserManager : IDomainManager<SystemUser>
{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemUser?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto);
    Task<SystemUser?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemUser> AddAsync(SystemUser entity);
    Task<SystemUser> UpdateAsync(SystemUser entity, SystemUserUpdateDto dto);
    Task<SystemUser?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemUser, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemUser, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemUserItemDto>> FilterAsync(SystemUserFilterDto filter);
    Task<SystemUser?> DeleteAsync(SystemUser entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
    string GetCaptcha(int length = 6);
}
