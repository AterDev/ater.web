namespace Http.Application.Interface;
/// <summary>
/// 基础查询接口
/// </summary>
public interface IDataStoreQuery<TId, TFilter>
    where TFilter : class
{
    /// <summary>
    /// id查询 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TDto> FindAsync<TDto>(TId id);
    Task<TDto> FindAsync<TDto>(TFilter filter);
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="tracking"></param>
    /// <returns></returns>
    Task<List<TItem>> ListAsync<TItem>(TFilter filter);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<PageList<TItem>> PageListAsync<TItem>(TFilter filter);
}

public interface IDataStoreQuery<TFilter> : IDataStoreQuery<Guid, TFilter>
    where TFilter : class
{ }