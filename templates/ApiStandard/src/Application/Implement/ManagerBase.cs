using Ater.Web.Abstraction.EntityFramework;

using Entity.SystemMod;

using EntityFramework.DBProvider;

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Implement;

/// <summary>
/// Manager base class
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TUpdate"></typeparam>
/// <typeparam name="TFilter"></typeparam>
/// <typeparam name="TItem"></typeparam>
public partial class ManagerBase<TEntity, TUpdate, TFilter, TItem>
    where TEntity : class, IEntityBase
    where TFilter : FilterBase
{
    #region Properties and Fields
    protected readonly ILogger _logger;
    public IUserContext? UserContext { get; private set; }

    /// <summary>
    /// 自动日志类型
    /// </summary>
    public LogActionType AutoLogType { get; private set; } = LogActionType.None;

    /// <summary>
    /// 实体的只读仓储实现
    /// </summary>
    public QuerySet<QueryDbContext, TEntity> Query { get; init; }
    /// <summary>
    /// 实体的可写仓储实现
    /// </summary>
    public CommandSet<CommandDbContext, TEntity> Command { get; init; }
    public IQueryable<TEntity> Queryable { get; set; }

    public CommandDbContext CommandContext { get; init; }

    public QueryDbContext QueryContext { get; init; }
    /// <summary>
    /// 是否自动保存(调用SaveChanges)
    /// </summary>
    public bool AutoSave { get; set; } = true;
    /// <summary>
    /// 错误信息
    /// </summary>
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    ///错误状态码
    /// </summary>
    public int ErrorStatus { get; set; }

    public DatabaseFacade Database { get; init; }
    #endregion

    public ManagerBase(DataAccessContext<TEntity> dataAccessContext, ILogger logger)
    {
        Query = dataAccessContext.QuerySet();
        Command = dataAccessContext.CommandSet();
        Queryable = Query.Queryable;
        Database = Command.Database;
        _logger = logger;
        CommandContext = dataAccessContext.CommandContext;
        QueryContext = dataAccessContext.QueryContext;
    }

    /// <summary>
    /// 在修改前查询对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigations">include navigations</param>
    /// <returns></returns>
    public virtual async Task<TEntity?> GetCurrentAsync(Guid id, params string[]? navigations)
    {
        return await Command.FindAsync(e => e.Id == id, navigations);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        TEntity res = await Command.CreateAsync(entity);
        await AutoSaveAsync();

        if (AutoLogType is LogActionType.Add or LogActionType.All or LogActionType.AddOrUpdate)
        {
            await SaveToLogAsync(entity, UserActionType.Add);
        }
        return res;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, TUpdate dto)
    {
        _ = entity.Merge(dto, true);
        entity.UpdatedTime = DateTimeOffset.UtcNow;
        TEntity res = Command.Update(entity);
        await AutoSaveAsync();
        if (AutoLogType is LogActionType.Update or LogActionType.All or LogActionType.AddOrUpdate)
        {
            await SaveToLogAsync(entity, UserActionType.Update);
        }
        return res;
    }

    public virtual async Task<TEntity?> DeleteAsync(TEntity entity, bool softDelete = true)
    {
        Command.EnableSoftDelete = softDelete;
        TEntity? res = Command.Remove(entity);
        await AutoSaveAsync();

        if (AutoLogType is LogActionType.Delete or LogActionType.All)
        {
            await SaveToLogAsync(entity, UserActionType.Delete);
        }
        return res;
    }

    public virtual async Task<TEntity?> FindAsync(Guid id)
    {
        return await Query.FindAsync(q => q.Id == id);
    }

    public virtual async Task<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp = null) where TDto : class
    {
        return await Query.FindAsync<TDto>(whereExp);
    }

    /// <summary>
    /// id是否存在
    /// </summary>
    /// <param name="id">主键id</param>
    /// <returns></returns>
    public virtual async Task<bool> ExistAsync(Guid id)
    {
        return await Query.Db.AnyAsync(q => q.Id == id);
    }

    /// <summary>
    /// 条件查询列表
    /// </summary>
    /// <typeparam name="TDto">返回类型</typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<List<TDto>> ListAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp = null) where TDto : class
    {
        return await Query.ListAsync<TDto>(whereExp);
    }
    public virtual async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? whereExp = null)
    {
        return await Query.ListAsync(whereExp);
    }

    /// <summary>
    /// 分页筛选，需要重写该方法
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<PageList<TItem>> FilterAsync(TFilter filter)
    {
        return await Query.FilterAsync<TItem>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 加载导航数据
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <returns></returns>
    public async Task LoadAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> propertyExpression) where TProperty : class
    {
        await CommandContext.Entry(entity).Reference(propertyExpression).LoadAsync();
    }

    /// <summary>
    /// 加载关联数据
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <returns></returns>
    public async Task LoadManyAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
    {
        await CommandContext.Entry(entity).Collection(propertyExpression).LoadAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Command.SaveChangesAsync();
    }

    private async Task AutoSaveAsync()
    {
        if (AutoSave)
        {
            _ = await SaveChangesAsync();
        }
    }

    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="actionType"></param>
    /// <returns></returns>
    private async Task SaveToLogAsync(TEntity entity, UserActionType actionType)
    {
        if (UserContext == null)
        {
            _logger.LogWarning("UserContext is null, can't save log");
            return;
        }

        var route = UserContext.GetHttpContext()?.Request.Path.Value;
        var description = string.Empty;
        var targetName = entity.GetType().Name;

        if (UserContext.IsAdmin)
        {
            // 管理员日志
            // 使用SystemMod时生效
            var log = SystemLogs.NewLog(UserContext.Username ?? "", UserContext.UserId, targetName, actionType, route, description);
            var taskQueue = WebAppContext.GetScopeService<EntityTaskQueue<SystemLogs>>();
            if (taskQueue != null)
            {
                await taskQueue.AddItemAsync(log);
            }
        }
        else
        {
            // 用户日志
            var log = UserLogs.NewLog(UserContext.Username ?? "", UserContext.UserId, targetName, actionType, route, description);
            var taskQueue = WebAppContext.GetScopeService<EntityTaskQueue<UserLogs>>();
            if (taskQueue != null)
            {
                await taskQueue.AddItemAsync(log);
            }
        }
    }
}
