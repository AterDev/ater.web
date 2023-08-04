using Share.Models.SystemMenuDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemMenuManager : IDomainManager<SystemMenu>
{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemMenu?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemMenu> CreateNewEntityAsync(SystemMenuAddDto dto);

    /// <summary>
    /// 获取当前对象,通常是在修改前进行查询
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigations"></param>
    /// <returns></returns>
    Task<SystemMenu?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemMenu> AddAsync(SystemMenu entity);
    Task<SystemMenu> UpdateAsync(SystemMenu entity, SystemMenuUpdateDto dto);
    Task<SystemMenu?> FindAsync(Guid id);
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemMenu, bool>>? whereExp) where TDto : class;
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<List<SystemMenu>> ListAsync(Expression<Func<SystemMenu, bool>>? whereExp = null);

    Task<SystemMenu?> DeleteAsync(SystemMenu entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
    Task<bool> SyncSystemMenusAsync(List<SystemMenuSyncDto> menus);
    Task<PageList<SystemMenu>> FilterAsync(SystemMenuFilterDto filter);
}
