using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Application.IManager;

/// <summary>
/// 仓储数据管理接口
/// </summary>
public interface IDomainManager<TEntity>
    where TEntity : class, IEntityBase
{
    DataStoreContext Stores { get; init; }
    QuerySet<TEntity> Query { get; init; }
    CommandSet<TEntity> Command { get; init; }
    public IQueryable<TEntity> Queryable { get; set; }
    public bool AutoSave { get; set; }
    public DatabaseFacade Database { get; init; }
}