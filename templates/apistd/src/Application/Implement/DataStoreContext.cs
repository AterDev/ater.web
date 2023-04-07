using Core.Entities.SystemEntities;

namespace Application.Implement;
public class DataStoreContext
{
    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }

    public QuerySet<SystemRole> SystemRoleQuery { get; init; }
    public QuerySet<SystemUser> SystemUserQuery { get; init; }
    public CommandSet<SystemRole> SystemRoleCommand { get; init; }
    public CommandSet<SystemUser> SystemUserCommand { get; init; }


    /// <summary>
    /// 绑在对象
    /// </summary>
    private readonly Dictionary<string, object> SetCache = new();

    public DataStoreContext(
        SystemRoleQueryStore systemRoleQuery,
        SystemUserQueryStore systemUserQuery,
        SystemRoleCommandStore systemRoleCommand,
        SystemUserCommandStore systemUserCommand,

        QueryDbContext queryDbContext,
        CommandDbContext commandDbContext
    )
    {
        QueryContext = queryDbContext;
        CommandContext = commandDbContext;
        SystemRoleQuery = systemRoleQuery;
        AddCache(SystemRoleQuery);
        SystemUserQuery = systemUserQuery;
        AddCache(SystemUserQuery);
        SystemRoleCommand = systemRoleCommand;
        AddCache(SystemRoleCommand);
        SystemUserCommand = systemUserCommand;
        AddCache(SystemUserCommand);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await CommandContext.SaveChangesAsync();
    }

    public QuerySet<TEntity> QuerySet<TEntity>() where TEntity : EntityBase
    {
        string typename = typeof(TEntity).Name + "QueryStore";
        object set = GetSet(typename);
        return set == null ? throw new ArgumentNullException($"{typename} class object not found") : (QuerySet<TEntity>)set;
    }
    public CommandSet<TEntity> CommandSet<TEntity>() where TEntity : EntityBase
    {
        string typename = typeof(TEntity).Name + "CommandStore";
        object set = GetSet(typename);
        return set == null ? throw new ArgumentNullException($"{typename} class object not found") : (CommandSet<TEntity>)set;
    }

    private void AddCache(object set)
    {
        string typeName = set.GetType().Name;
        if (!SetCache.ContainsKey(typeName))
        {
            SetCache.Add(typeName, set);
        }
    }

    private object GetSet(string type)
    {
        return SetCache[type];
    }
}
