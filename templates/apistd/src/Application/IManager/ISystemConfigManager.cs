using Share.Models.SystemConfigDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemConfigManager :IDomainManager<SystemConfig>{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemConfig?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemConfig> CreateNewEntityAsync(SystemConfigAddDto dto);
    Task<SystemConfig?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemConfig> AddAsync(SystemConfig entity);
    Task<SystemConfig> UpdateAsync(SystemConfig entity, SystemConfigUpdateDto dto);
    Task<SystemConfig?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemConfig, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemConfig, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemConfigItemDto>> FilterAsync(SystemConfigFilterDto filter);
    Task<SystemConfig?> DeleteAsync(SystemConfig entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
