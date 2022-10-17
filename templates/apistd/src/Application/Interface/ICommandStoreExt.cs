namespace Application.Interface;
public interface ICommandStoreExt<TId, TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="chunk"></param>
    /// <returns></returns>
    Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities, int? chunk = 50);

    /// <summary>
    /// 条件更新
    /// </summary>
    /// <typeparam name="TUpdate"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<int> UpdateRangeAsync<TUpdate>(Expression<Func<TEntity, bool>> whereExp, TUpdate dto);
}

public interface ICommandStoreExt<TEntity> : ICommandStoreExt<Guid, TEntity>
     where TEntity : EntityBase
{ }
