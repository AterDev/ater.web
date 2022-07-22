﻿namespace Application.Implement;
/// <summary>
/// 只读仓储
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public class QueryDataStoreBase<TContext, TEntity> :
    IDataStoreQuery<TEntity>, IDataStoreQueryExt<TEntity>
    where TContext : DbContext
    where TEntity : EntityBase
{
    private readonly TContext _context;
    protected readonly ILogger _logger;
    /// <summary>
    /// 当前实体DbSet
    /// </summary>
    protected readonly DbSet<TEntity> _db;
    public DbSet<TEntity> Db { get => _db; }
    public IQueryable<TEntity> _query;


    public QueryDataStoreBase(TContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        _db = _context.Set<TEntity>();
        _query = _db.AsQueryable();
    }

    private void ResetQuery()
    {
        _query = _db.AsQueryable();
    }

    public virtual async Task<TDto?> FindAsync<TDto>(Guid id)
        where TDto : class
    {
        var res = await _query.Where(d => d.Id == id)
            .AsNoTracking()
            .ProjectTo<TDto>()
            .FirstOrDefaultAsync();
        ResetQuery();
        return res;
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp)
        where TDto : class
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        var res = await _query.Where(whereExp)
            .ProjectTo<TDto>()
            .FirstOrDefaultAsync();
        ResetQuery();
        return res;
    }

    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<List<TItem>> ListAsync<TItem>(Expression<Func<TEntity, bool>>? whereExp)
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        var res = await _query.Where(whereExp)
            .ProjectTo<TItem>()
            .ToListAsync();
        ResetQuery();
        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public virtual async Task<PageList<TItem>> PageListAsync<TItem>(Expression<Func<TEntity, bool>>? whereExp, int pageIndex = 1, int pageSize = 12)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 0) pageSize = 12;
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;

        var count = _query.Count();
        var data = await _query.Take(pageSize)
            .Skip((pageIndex - 1) * pageSize)
            .ProjectTo<TItem>()
            .ToListAsync();
        ResetQuery();
        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = pageIndex
        };
    }

    /// <summary>
    /// 分页筛选
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="order"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public virtual async Task<PageList<TItem>> FilterAsync<TItem>(Expression<Func<TEntity, bool>> whereExp, Dictionary<string, bool>? order, int pageIndex = 1, int pageSize = 12)
    {
        if (pageIndex < 1) pageIndex = 1;
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        _query = _query.Where(whereExp);

        if (order != null)
        {
            _query = _query.OrderBy(order);
        }
        var count = _query.Count();
        var data = await _query.Take(pageSize)
            .Skip((pageIndex - 1) * pageSize)
            .ProjectTo<TItem>()
            .ToListAsync();
        ResetQuery();
        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = pageIndex
        };
    }
}


public class QuerySet<TEntity> : QueryDataStoreBase<QueryDbContext, TEntity>
    where TEntity : EntityBase
{
    public QuerySet(QueryDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
