namespace Application.Interface;

/// <summary>
/// 仓储数据管理接口
/// </summary>
public interface IDomainManager<TEntity>
    where TEntity : EntityBase
{
    DataStoreContext Stores { get; init; }
    QuerySet<TEntity> Query { get; init; }
    CommandSet<TEntity> Command { get; init; }

    /// <summary>
    /// 获取当前对象,通常是在修改前进行查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetCurrent(Guid id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid id, TEntity entity);
    Task<TEntity?> DeleteAsync(Guid id);

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(Guid id) where TDto : class;
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="order"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PageList<TItem>> FilterAsync<TItem>(Expression<Func<TEntity, bool>> whereExp, Dictionary<string, bool>? order = null, int? pageIndex = 1, int? pageSize = 12);
}
