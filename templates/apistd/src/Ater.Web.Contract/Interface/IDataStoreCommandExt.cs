namespace Ater.Web.Contract.Interface;
public interface IDataStoreCommandExt<TId, TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities, int? chunk = 50);

    /// <summary>
    /// 批量更新，覆盖
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> UpdateRangeAsync<TUpdate>(Expression<Func<TEntity, bool>> whereExp, TUpdate dto);

    /// <summary>
    /// 批量更新,部分字段
    /// </summary>
    /// <typeparam name="TEdit"></typeparam>
    /// <param name="ids"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<int> EditRangeAsync<TUpdate>(List<TId> ids, TUpdate dto);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteRangeAsync(List<TId> ids);

    Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExp);
}

public interface IDataStoreCommandExt<TEntity> : IDataStoreCommandExt<Guid, TEntity>
     where TEntity : EntityBase
{ }
