﻿namespace Http.Application.Interface;
/// <summary>
/// 基础查询接口
/// </summary>
public interface IDataStoreQuery<TId, TEntity, TFilter>
    where TFilter : FilterBase
    where TEntity : EntityBase
{
    /// <summary>
    /// id查询 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TDto?> FindAsync<TDto>(TId id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp);
    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    Task<List<TItem>> ListAsync<TItem>(Expression<Func<TEntity, bool>>? whereExp);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<PageList<TItem>> PageListAsync<TItem>(TFilter filter);
}

public interface IDataStoreQuery<TEntity, TFilter> : IDataStoreQuery<Guid, TEntity, TFilter>
    where TFilter : FilterBase
    where TEntity : EntityBase
{ }