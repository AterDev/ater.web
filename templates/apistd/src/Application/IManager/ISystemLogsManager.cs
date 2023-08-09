using Share.Models.SystemLogsDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemLogsManager : IDomainManager<SystemLogs>
{
	/// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemLogs?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemLogs> CreateNewEntityAsync(SystemLogsAddDto dto);

    /// <summary>
    /// 获取当前对象,通常是在修改前进行查询
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigations"></param>
    /// <returns></returns>
    Task<SystemLogs?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemLogs> AddAsync(SystemLogs entity);
    Task<SystemLogs> UpdateAsync(SystemLogs entity, SystemLogsUpdateDto dto);
    Task<SystemLogs?> FindAsync(Guid id);
    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemLogs, bool>>? whereExp) where TDto : class;
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemLogs, bool>>? whereExp = null) where TDto : class;

    Task<List<SystemLogs>> ListAsync(Expression<Func<SystemLogs, bool>>? whereExp = null);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<PageList<SystemLogsItemDto>> FilterAsync(SystemLogsFilterDto filter);
    Task<SystemLogs?> DeleteAsync(SystemLogs entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
