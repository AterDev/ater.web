using Share.Models.SystemPermissionGroupDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemPermissionGroupManager : IDomainManager<SystemPermissionGroup>
{
	/// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemPermissionGroup?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemPermissionGroup> CreateNewEntityAsync(SystemPermissionGroupAddDto dto);

    /// <summary>
    /// 获取当前对象,通常是在修改前进行查询
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigations"></param>
    /// <returns></returns>
    Task<SystemPermissionGroup?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemPermissionGroup> AddAsync(SystemPermissionGroup entity);
    Task<SystemPermissionGroup> UpdateAsync(SystemPermissionGroup entity, SystemPermissionGroupUpdateDto dto);
    Task<SystemPermissionGroup?> FindAsync(Guid id);
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemPermissionGroup, bool>>? whereExp) where TDto : class;
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemPermissionGroup, bool>>? whereExp = null) where TDto : class;

    Task<List<SystemPermissionGroup>> ListAsync(Expression<Func<SystemPermissionGroup, bool>>? whereExp = null);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<PageList<SystemPermissionGroupItemDto>> FilterAsync(SystemPermissionGroupFilterDto filter);
    Task<SystemPermissionGroup?> DeleteAsync(SystemPermissionGroup entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
