using Core.Utils;
using Microsoft.Extensions.Logging;

namespace Ater.Web.Contract.Implement;
/// <summary>
/// 只读仓储
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public class DataStoreQueryBase<TContext, TEntity, TFilter> :
    IDataStoreQuery<TEntity, TFilter>, IDataStoreQueryExt<TEntity, TFilter>
    where TContext : DbContext
    where TEntity : EntityBase
    where TFilter : FilterBase
{
    private readonly TContext _context;
    protected readonly ILogger _logger;
    /// <summary>
    /// 当前实体DbSet
    /// </summary>
    protected readonly DbSet<TEntity> _db;
    public DbSet<TEntity> Db { get => _db; }
    public IQueryable<TEntity> _query;


    public DataStoreQueryBase(TContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        _db = _context.Set<TEntity>();
        _query = _db.AsQueryable();
    }

    public virtual async Task<TDto?> FindAsync<TDto>(Guid id)
    {
        return await _db.Where(d => d.Id == id)
            .AsNoTracking()
            .ProjectTo<TDto>()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp)
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        return await _db.Where(whereExp)
            .ProjectTo<TDto>()
            .FirstOrDefaultAsync();
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
        return await _db.Where(whereExp)
            .ProjectTo<TItem>()
            .ToListAsync();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<PageList<TItem>> PageListAsync<TItem>(TFilter filter)
    {
        if (filter.PageIndex is null or < 1) filter.PageIndex = 1;
        if (filter.PageSize is null or < 1) filter.PageSize = 12;

        var count = _query.Count();
        var data = await _query.Take(filter.PageSize.Value)
            .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
            .ProjectTo<TItem>()
            .ToListAsync();

        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = filter.PageIndex.Value
        };
    }

    /// <summary>
    /// 分页筛选
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="filter"></param>
    /// <param name="order">排序</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<PageList<TItem>> Filter<TItem>(TFilter filter, Dictionary<string, bool>? order)
    {
        if (filter.PageIndex is null or < 1) filter.PageIndex = 1;
        if (filter.PageSize is null or < 1) filter.PageSize = 12;

        if (order != null)
        {
            _query = _query.OrderBy(order);
        }
        var count = _query.Count();
        var data = await _query.Take(filter.PageSize.Value)
            .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
            .ProjectTo<TItem>()
            .ToListAsync();

        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = filter.PageIndex.Value
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
    public virtual async Task<PageList<TItem>> Filter<TItem>(Expression<Func<TEntity, bool>> whereExp, Dictionary<string, bool>? order, int pageIndex = 1, int pageSize = 12)
    {
        if (pageIndex < 1) pageIndex = 1;
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

        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = pageIndex
        };

    }
}
