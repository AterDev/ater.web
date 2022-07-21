namespace Application.Manager;

public class DomainManagerBase<TEntity> : IDomainManager<TEntity>
    where TEntity : EntityBase
{
    public DataStoreContext Stores { get; init; }
    public QuerySet<TEntity> Query { get; init; }
    public CommandSet<TEntity> Command { get; init; }

    public DomainManagerBase(DataStoreContext storeContext)
    {
        Stores = storeContext;
        Query = Stores.QuerySet<TEntity>();
        Command = Stores.CommandSet<TEntity>();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Stores.SaveChangesAsync();
    }

    /// <summary>
    /// 在修改前查询对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TEntity?> GetCurrent(Guid id)
    {
        return await Command.FindAsync(e => e.Id == id);
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        return await Command.CreateAsync(entity);
    }

    public virtual async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        return await Command.UpdateAsync(id, entity);
    }

    public virtual async Task<TEntity?> DeleteAsync(Guid id)
    {
        return await Command.DeleteAsync(id);
    }

    public virtual async Task<TDto?> FindAsync<TDto>(Guid id) where TDto : class
    {
        return await Query.FindAsync<TDto>(id);
    }

    /// <summary>
    /// 分页筛选，需要重写该方法
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public virtual async Task<PageList<TItem>> FilterAsync<TItem, TFilter>(TFilter filter, int? pageIndex = 1, int? pageSize = 12)
        where TFilter : FilterBase
    {
        Expression<Func<TEntity, bool>> exp = e => true;
        return await Query.FilterAsync<TItem>(exp, filter.OrderBy, pageIndex ?? 1, pageSize ?? 12);
    }

}
