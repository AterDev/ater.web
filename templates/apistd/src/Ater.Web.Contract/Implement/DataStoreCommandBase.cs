using Ater.Web.Contract.Interface;
using Microsoft.Extensions.Logging;

namespace Ater.Web.Contract.Implement;
public class DataStoreCommandBase<TContext, TEntity> : IDataStoreCommand<TEntity>, IDataStoreCommandExt<TEntity>
    where TContext : DbContext
    where TEntity : class
{
    private readonly TContext _context;
    protected readonly ILogger _logger;
    /// <summary>
    /// 当前实体DbSet
    /// </summary>
    protected readonly DbSet<TEntity> _db;

    public DataStoreCommandBase(TContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        _db = _context.Set<TEntity>();
    }

    public Task<TEntity> CreateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> EditAsync<TEdit>(Guid id, TEdit dto)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> EditRangeAsync<TEdit>(List<Guid> ids, TEdit dto)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteRangeAsync(List<Guid> ids)
    {
        throw new NotImplementedException();
    }
}
