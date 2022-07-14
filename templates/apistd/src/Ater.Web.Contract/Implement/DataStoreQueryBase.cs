using Microsoft.Extensions.Logging;

namespace Ater.Web.Contract.Implement;
/// <summary>
/// 只读仓储
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public class DataStoreQueryBase<TContext, TEntity, TFilter> :
    IDataStoreQuery<TFilter>, IDataStoreQueryExt<TEntity, TFilter>
    where TContext : DbContext
    where TEntity : class
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

    public Task<TDto> FindAsync<TDto>(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TDto> FindAsync<TDto>(TFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<List<TItem>> ListAsync<TItem>(TFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<PageList<TItem>> PageListAsync<TItem>(TFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<PageList<TItem>> Filter<TItem>(TFilter filter, Dictionary<string, bool> order)
    {
        throw new NotImplementedException();
    }

    public Task<PageList<TItem>> Filter<TItem>(Expression<Func<TEntity, bool>> whereExp, Dictionary<string, bool>? order, int? pageIndex = 1, int? pageSize = 12)
    {
        throw new NotImplementedException();
    }
}
