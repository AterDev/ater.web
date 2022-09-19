using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Implement;
public class CommandStoreBase<TContext, TEntity> : ICommandStore<TEntity>, ICommandStoreExt<TEntity>
    where TContext : DbContext
    where TEntity : EntityBase
{
    private readonly TContext _context;
    protected readonly ILogger _logger;
    /// <summary>
    /// 当前实体DbSet
    /// </summary>
    protected readonly DbSet<TEntity> _db;
    public DbSet<TEntity> Db => _db;
    public TContext Context => _context;
    public DatabaseFacade Database { get; init; }
    public bool EnableSoftDelete { get; set; } = true;

    //public TEntity CurrentEntity { get; }

    public CommandStoreBase(TContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        _db = _context.Set<TEntity>();
        Database = _context.Database;
    }

    public virtual async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>>? whereExp, string[]? navigations = null)
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        var _query = _db.Where(whereExp).AsQueryable();
        if (navigations != null)
        {
            foreach (var item in navigations)
            {
                _query = _query.Include(item);
            }
        }
        return await _query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// 列表条件查询
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? whereExp)
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        whereExp ??= exp;
        var res = await _db.Where(whereExp)
            .ToListAsync();
        return res;
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _db.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual TEntity Update(TEntity entity)
    {
        _db.Update(entity);
        return entity;
    }

    /// <summary>
    /// 删除实体,若未找到，返回null
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual TEntity? Remove(TEntity entity)
    {
        if (EnableSoftDelete)
        {
            entity.IsDeleted = true;
        }
        else
        {
            _db.Remove(entity!);
        }
        return entity;
    }


    /// <summary>
    /// 批量创建
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities, int? chunk = 50)
    {
        if (chunk != null && entities.Count > chunk)
        {
            entities.Chunk(chunk.Value).ToList()
                .ForEach(block =>
                {
                    _db.AddRange(block);
                    _context.SaveChanges();
                });
        }
        else
        {
            await _db.AddRangeAsync(entities);
            await SaveChangeAsync();
        }
        return entities;
    }

    /// <summary>
    /// 条件更新
    /// </summary>
    /// <typeparam name="TUpdate"></typeparam>
    /// <param name="whereExp"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateRangeAsync<TUpdate>(Expression<Func<TEntity, bool>> whereExp, TUpdate dto)
    {
        //return await _db.Where(whereExp).ExecuteUpdateAsync(d => d.SetProperty(d => d.Id, d => Guid.NewGuid()));
        throw new NotImplementedException();
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteRangeAsync(List<Guid> ids)
    {
        return await _db.Where(d => ids.Contains(d.Id)).ExecuteDeleteAsync();
        throw new NotImplementedException();

    }

    /// <summary>
    /// 条件删除
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExp)
    {
        return await _db.Where(whereExp).ExecuteDeleteAsync();
        throw new NotImplementedException();

    }
}
public class CommandSet<TEntity> : CommandStoreBase<CommandDbContext, TEntity>
    where TEntity : EntityBase
{
    public CommandSet(CommandDbContext context, ILogger logger) : base(context, logger)
    {
    }
}