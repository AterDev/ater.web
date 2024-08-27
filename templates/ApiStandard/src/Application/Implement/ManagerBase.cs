using Entity.SystemMod;

using EntityFramework.DBProvider;

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Implement;

/// <summary>
/// Manager base class
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TUpdate">更新DTO</typeparam>
/// <typeparam name="TFilter">筛选DTO</typeparam>
/// <typeparam name="TItem">列表元素DTO</typeparam>
public partial class ManagerBase<TEntity, TUpdate, TFilter, TItem> : ManagerBase<TEntity>
    where TEntity : class, IEntityBase
    where TFilter : FilterBase
{
    #region Properties and Fields
    protected IUserContext? UserContext { get; private set; }

    /// <summary>
    /// 自动日志类型
    /// </summary>
    protected LogActionType AutoLogType { get; private set; } = LogActionType.None;

    /// <summary>
    /// 全局筛选
    /// </summary>
    public bool EnableGlobalQuery { get; set; } = true;

    /// <summary>
    /// 是否自动保存(调用SaveChanges)
    /// </summary>
    protected bool AutoSave { get; set; } = true;
    /// <summary>
    /// 错误信息
    /// </summary>
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    ///错误状态码
    /// </summary>
    public int ErrorStatus { get; set; }
    /// <summary>
    /// 当前实体
    /// </summary>
    public TEntity? CurrentEntity { get; set; }
    #endregion

    public ManagerBase(DataAccessContext<TEntity> dataAccessContext, ILogger logger) : base(dataAccessContext, logger)
    {
        if (!EnableGlobalQuery)
        {
            Queryable = Queryable.IgnoreQueryFilters();
        }
    }

    /// <summary>
    /// 在修改前查询对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TEntity?> GetCurrentAsync(Guid id)
    {
        return await Command.FindAsync(id);
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TEntity?> FindAsync(Guid id)
    {
        var entity = await Query.FindAsync(id);
        if (entity != null)
        {
            Command.Attach(entity);
        }
        return entity;
    }

    /// <summary>
    /// 实体查询
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp = null) where TDto : class
    {
        var model = await Query.AsNoTracking()
            .Where(whereExp ?? (e => true))
            .ProjectTo<TDto>()
            .FirstOrDefaultAsync();

        if (typeof(TDto) is TEntity && model != null)
        {
            Command.Attach((model as TEntity)!);
        }
        return model;
    }

    /// <summary>
    /// id是否存在
    /// </summary>
    /// <param name="id">主键id</param>
    /// <returns></returns>
    public virtual async Task<bool> ExistAsync(Guid id)
    {
        return await Query.AnyAsync(q => q.Id == id);
    }

    /// <summary>
    /// 存在判断
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> whereExp)
    {
        return await Query.AnyAsync(whereExp);
    }

    /// <summary>
    /// 条件查询列表
    /// </summary>
    /// <typeparam name="TDto">返回类型</typeparam>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public virtual async Task<List<TDto>> ToListAsync<TDto>(Expression<Func<TEntity, bool>>? whereExp = null) where TDto : class
    {
        return await Query.AsNoTracking()
            .Where(whereExp ?? (e => true))
            .ProjectTo<TDto>()
            .ToListAsync();
    }

    public virtual async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? whereExp = null)
    {

        return await Query.AsNoTracking()
            .Where(whereExp ?? (e => true))
            .ToListAsync();
    }

    /// <summary>
    /// 分页筛选，需要重写该方法
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<PageList<TItem>> ToPageAsync(TFilter filter)
    {
        Queryable = filter.OrderBy != null
            ? Queryable.OrderBy(filter.OrderBy)
            : Queryable.OrderByDescending(t => t.CreatedTime);

        var count = Queryable.Count();
        List<TItem> data = await Queryable
            .AsNoTracking()
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<TItem>()
            .ToListAsync();

        ResetQuery();
        return new PageList<TItem>
        {
            Count = count,
            Data = data,
            PageIndex = filter.PageIndex
        };
    }

    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        await Command.AddAsync(entity);
        if (AutoSave)
        {
            return await SaveChangesAsync() > 0;
        }

        if (AutoLogType is LogActionType.Add or LogActionType.All or LogActionType.AddOrUpdate)
        {
            await SaveToLogAsync(UserActionType.Add);
        }
        return true;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">已跟踪的实体</param>
    /// <returns></returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        Command.Update(entity);
        if (AutoSave)
        {
            return await SaveChangesAsync() > 0;
        }

        if (AutoLogType is LogActionType.Update or LogActionType.All or LogActionType.AddOrUpdate)
        {
            await SaveToLogAsync(UserActionType.Update);
        }
        return true;
    }


    /// <summary>
    /// 更新关联数据
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="data"></param>
    public void UpdateRelation<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, List<TProperty> data) where TProperty : class
    {
        var currentValue = CommandContext.Entry(entity).Collection(propertyExpression).CurrentValue;
        if (currentValue != null && currentValue.Any())
        {
            CommandContext.RemoveRange(currentValue);
            CommandContext.Entry(entity).Collection(propertyExpression).CurrentValue = null;
        }
        CommandContext.AddRange(data);
    }

    /// <summary>
    /// 批量保存
    /// </summary>
    /// <param name="entityList"></param>
    /// <returns></returns>
    public async Task<bool> SaveAsync(List<TEntity> entityList)
    {
        var Ids = await Command.Select(e => e.Id).ToListAsync();
        // new entity by id
        var newEntities = entityList.Where(d => !Ids.Contains(d.Id)).ToList();

        var updateEntities = entityList.Where(d => Ids.Contains(d.Id)).ToList();
        var removeEntities = Ids.Where(d => !entityList.Select(e => e.Id).Contains(d)).ToList();

        if (newEntities.Any())
        {
            await Command.AddRangeAsync(newEntities);
        }
        if (updateEntities.Any())
        {
            Command.UpdateRange(updateEntities);
        }
        try
        {
            if (removeEntities.Any())
            {
                await Command.Where(d => removeEntities.Contains(d.Id)).ExecuteDeleteAsync();
            }
            _ = await SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddOrUpdateAsync");
            return false;
        }
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="ids">实体id</param>
    /// <param name="softDelete">是否软件删除</param>
    /// <returns></returns>
    public virtual async Task<bool?> DeleteAsync(List<Guid> ids, bool softDelete = true)
    {
        var res = 0;
        if (softDelete)
        {
            res = await Command.Where(d => ids.Contains(d.Id))
                .ExecuteUpdateAsync(d => d.SetProperty(d => d.IsDeleted, true));
        }
        else
        {
            res = await Command.Where(d => ids.Contains(d.Id)).ExecuteDeleteAsync();
        }

        if (AutoLogType is LogActionType.Delete or LogActionType.All)
        {
            await SaveToLogAsync(UserActionType.Delete);
        }
        return res > 0;
    }

    /// <summary>
    /// 加载导航数据
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <returns></returns>
    protected async Task LoadAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> propertyExpression) where TProperty : class
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
    protected async Task LoadManyAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
    {
        await CommandContext.Entry(entity).Collection(propertyExpression).LoadAsync();
    }

    protected async Task<int> SaveChangesAsync()
    {
        return await CommandContext.SaveChangesAsync();
    }

    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="actionType"></param>
    /// <returns></returns>
    private async Task SaveToLogAsync(UserActionType actionType)
    {
        if (UserContext == null)
        {
            _logger.LogWarning("UserContext is null, can't save log");
            return;
        }

        var route = UserContext.GetHttpContext()?.Request.Path.Value;
        var description = string.Empty;
        var targetName = typeof(TEntity).GetType().Name;

        if (UserContext.IsAdmin)
        {
            // 管理员日志
            // 使用SystemMod时生效
            var log = SystemLogs.NewLog(UserContext.Username ?? "", UserContext.UserId, targetName, actionType, route, description);
            var taskQueue = WebAppContext.GetScopeService<IEntityTaskQueue<SystemLogs>>();
            if (taskQueue != null)
            {
                await taskQueue.AddItemAsync(log);
            }
        }
        else
        {
            // 用户日志
            var log = UserLogs.NewLog(UserContext.Username ?? "", UserContext.UserId, targetName, actionType, route, description);
            var taskQueue = WebAppContext.GetScopeService<IEntityTaskQueue<UserLogs>>();
            if (taskQueue != null)
            {
                await taskQueue.AddItemAsync(log);
            }
        }
    }

    /// <summary>
    /// reset queryable
    /// </summary>
    private void ResetQuery()
    {
        Queryable = EnableGlobalQuery
            ? Query.AsQueryable()
            : Queryable.IgnoreQueryFilters().AsQueryable();
    }
}

/// <summary>
/// Manager base with entity
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class ManagerBase<TEntity> : ManagerBase where TEntity : class, IEntityBase
{
    protected CommandDbContext CommandContext { get; init; }
    protected QueryDbContext QueryContext { get; init; }
    protected DatabaseFacade Database { get; init; }
    /// <summary>
    /// 实体的只读仓储实现
    /// </summary>
    protected DbSet<TEntity> Query { get; init; }
    /// <summary>
    /// 实体的可写仓储实现
    /// </summary>
    protected DbSet<TEntity> Command { get; init; }
    protected IQueryable<TEntity> Queryable { get; set; }

    public ManagerBase(DataAccessContext<TEntity> dataAccessContext, ILogger logger) : base(logger)
    {
        CommandContext = dataAccessContext.CommandContext;
        QueryContext = dataAccessContext.QueryContext;
        Database = CommandContext.Database;
        Query = QueryContext.Set<TEntity>();
        Command = CommandContext.Set<TEntity>();
        Queryable = Query.AsNoTracking().AsQueryable();
    }
}

/// <summary>
/// Manager base without entity
/// </summary>
/// <param name="logger"></param>
public class ManagerBase(ILogger logger)
{
    protected readonly ILogger _logger = logger;
}