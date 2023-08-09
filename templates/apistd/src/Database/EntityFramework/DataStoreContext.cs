using Entity;
using Entity.CMSEntities;
using Entity.SystemEntities;
namespace EntityFramework;
public class DataStoreContext
{
    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }



    /// <summary>
    /// 绑在对象
    /// </summary>
    private readonly Dictionary<string, object> StoreCache = new();

    public DataStoreContext(
        BlogQueryStore blogQuery,
        SystemConfigQueryStore systemConfigQuery,
        SystemLogsQueryStore systemLogsQuery,
        SystemMenuQueryStore systemMenuQuery,
        SystemPermissionGroupQueryStore systemPermissionGroupQuery,
        SystemPermissionQueryStore systemPermissionQuery,
        SystemRoleQueryStore systemRoleQuery,
        SystemUserQueryStore systemUserQuery,
        UserQueryStore userQuery,
        BlogCommandStore blogCommand,
        SystemConfigCommandStore systemConfigCommand,
        SystemLogsCommandStore systemLogsCommand,
        SystemMenuCommandStore systemMenuCommand,
        SystemPermissionCommandStore systemPermissionCommand,
        SystemPermissionGroupCommandStore systemPermissionGroupCommand,
        SystemRoleCommandStore systemRoleCommand,
        SystemUserCommandStore systemUserCommand,
        UserCommandStore userCommand,

        QueryDbContext queryDbContext,
        CommandDbContext commandDbContext
    )
    {
        QueryContext = queryDbContext;
        CommandContext = commandDbContext;
        AddCache(blogQuery);
        AddCache(systemConfigQuery);
        AddCache(systemLogsQuery);
        AddCache(systemMenuQuery);
        AddCache(systemPermissionGroupQuery);
        AddCache(systemPermissionQuery);
        AddCache(systemRoleQuery);
        AddCache(systemUserQuery);
        AddCache(userQuery);
        AddCache(blogCommand);
        AddCache(systemConfigCommand);
        AddCache(systemLogsCommand);
        AddCache(systemMenuCommand);
        AddCache(systemPermissionCommand);
        AddCache(systemPermissionGroupCommand);
        AddCache(systemRoleCommand);
        AddCache(systemUserCommand);
        AddCache(userCommand);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await CommandContext.SaveChangesAsync();
    }

    public QuerySet<TEntity> QuerySet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "QueryStore";
        var set = GetSet(typename);
        return set == null 
            ? throw new ArgumentNullException($"{typename} class object not found") 
            : (QuerySet<TEntity>)set;
    }
    public CommandSet<TEntity> CommandSet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "CommandStore";
        var set = GetSet(typename);
        return set == null 
            ? throw new ArgumentNullException($"{typename} class object not found") 
            : (CommandSet<TEntity>)set;
    }

    private void AddCache(object set)
    {
        var typeName = set.GetType().Name;
        if (!StoreCache.ContainsKey(typeName))
        {
            StoreCache.Add(typeName, set);
        }
    }

    private object GetSet(string type)
    {
        return StoreCache[type];
    }
}
