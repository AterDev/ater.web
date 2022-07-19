namespace Http.Application.Interface;

/// <summary>
/// 仓储命令
/// </summary>
public interface IDataStoreCommand<TId, TEntity>
    where TEntity : class
{
    /// <summary>
    /// 创建模型
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// 整体更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TId id, TEntity entity);

    /// <summary>
    /// 部分更新
    /// </summary>
    /// <typeparam name="TEdit"></typeparam>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<TEntity> EditAsync<TEdit>(TId id, TEdit dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> DeleteAsync(TId id);
}
public interface IDataStoreCommand<TEntity> : IDataStoreCommand<Guid, TEntity>
    where TEntity : class
{ }
