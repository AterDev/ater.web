using Share.Models.SystemConfigDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemConfigManager : IDomainManager<SystemConfig>
{
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

    /// <summary>
    /// 获取当前对象,通常是在修改前进行查询
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigations"></param>
    /// <returns></returns>
    Task<SystemConfig?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemConfig> AddAsync(SystemConfig entity);
    Task<SystemConfig> UpdateAsync(SystemConfig entity, SystemConfigUpdateDto dto);
    Task<SystemConfig?> FindAsync(Guid id);
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemConfig, bool>>? whereExp) where TDto : class;
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemConfig, bool>>? whereExp = null) where TDto : class;

    Task<List<SystemConfig>> ListAsync(Expression<Func<SystemConfig, bool>>? whereExp = null);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<PageList<SystemConfigItemDto>> FilterAsync(SystemConfigFilterDto filter);
    Task<SystemConfig?> DeleteAsync(SystemConfig entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
    Task<Dictionary<string, List<EnumDictionary>>> GetEnumConfigsAsync();
}
