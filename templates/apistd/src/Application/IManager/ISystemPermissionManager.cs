using Share.Models.SystemPermissionDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemPermissionManager :IDomainManager<SystemPermission>{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemPermission?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemPermission> CreateNewEntityAsync(SystemPermissionAddDto dto);
    Task<SystemPermission?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemPermission> AddAsync(SystemPermission entity);
    Task<SystemPermission> UpdateAsync(SystemPermission entity, SystemPermissionUpdateDto dto);
    Task<SystemPermission?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemPermission, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemPermission, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemPermissionItemDto>> FilterAsync(SystemPermissionFilterDto filter);
    Task<SystemPermission?> DeleteAsync(SystemPermission entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
