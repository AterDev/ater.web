using Core.Models;
namespace Http.API.Infrastructure;

public interface IRestAPI<TEntity, TFilter, TUpdate>
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    Task<ActionResult<TEntity>> AddAsync<TAdd>(TAdd form);

    /// <summary>
    /// 分页筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<ActionResult<PageList<TItem>>> FilterAsync<TItem>(TFilter filter);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ActionResult<TEntity?>> GetDetailAsync(Guid id);
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="form"></param>
    /// <returns></returns>
    Task<ActionResult<TEntity?>> UpdateAsync(Guid id, TUpdate form);
}
