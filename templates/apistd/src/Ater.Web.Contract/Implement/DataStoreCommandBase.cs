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

    public virtual async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _db.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 编辑实体
    /// </summary>
    /// <typeparam name="TEdit"></typeparam>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<TEntity> EditAsync<TEdit>(Guid id, TEdit dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 删除实体,若未找到，返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<TEntity?> DeleteAsync(Guid id)
    {
        var entity = await _db.FindAsync(id);
        if (entity != null)
        {
            _db.Remove(entity!);
        }
        return entity;
    }

    /// <summary>
    /// 批量创建
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 批量编辑
    /// </summary>
    /// <typeparam name="TEdit"></typeparam>
    /// <param name="ids"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<TEntity>> EditRangeAsync<TEdit>(List<Guid> ids, TEdit dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public Task<int> DeleteRangeAsync(List<Guid> ids)
    {
        throw new NotImplementedException();
    }
}
