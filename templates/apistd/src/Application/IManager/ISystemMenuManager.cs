using Share.Models.SystemMenuDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemMenuManager :IDomainManager<SystemMenu>{
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
    Task<SystemMenu?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemMenu> AddAsync(SystemMenu entity);
    Task<SystemMenu> UpdateAsync(SystemMenu entity, SystemMenuUpdateDto dto);
    Task<SystemMenu?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemMenu, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemMenu, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemMenuItemDto>> FilterAsync(SystemMenuFilterDto filter);
    Task<SystemMenu?> DeleteAsync(SystemMenu entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
