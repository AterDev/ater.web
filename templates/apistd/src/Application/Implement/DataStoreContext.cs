using Core.Entities;
using Core.Entities.SystemEntities;
namespace Application.Implement;
public class DataStoreContext
{
    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }

    /// <summary>
    /// 绑定对象
    /// </summary>
    private readonly Dictionary<string, object> StoreCache = new();

    public DataStoreContext(
        SystemConfigQueryStore systemConfigQuery,
        SystemLogsQueryStore systemLogsQuery,
        SystemMenuQueryStore systemMenuQuery,
        SystemOrganizationQueryStore systemOrganizationQuery,
        SystemPermissionQueryStore systemPermissionQuery,
        SystemRoleQueryStore systemRoleQuery,
        SystemUserQueryStore systemUserQuery,
        UserQueryStore userQuery,
        SystemConfigCommandStore systemConfigCommand,
        SystemLogsCommandStore systemLogsCommand,
        SystemMenuCommandStore systemMenuCommand,
        SystemOrganizationCommandStore systemOrganizationCommand,
        SystemPermissionCommandStore systemPermissionCommand,
        SystemRoleCommandStore systemRoleCommand,
        SystemUserCommandStore systemUserCommand,
        UserCommandStore userCommand,
        QueryDbContext queryDbContext,
        CommandDbContext commandDbContext
    )
    {
        QueryContext = queryDbContext;
        CommandContext = commandDbContext;
        AddCache(systemConfigQuery);
        AddCache(systemLogsQuery);
        AddCache(systemMenuQuery);
        AddCache(systemOrganizationQuery);
        AddCache(systemPermissionQuery);
        AddCache(systemRoleQuery);
        AddCache(systemUserQuery);
        AddCache(userQuery);
        AddCache(systemConfigCommand);
        AddCache(systemLogsCommand);
        AddCache(systemMenuCommand);
        AddCache(systemOrganizationCommand);
        AddCache(systemPermissionCommand);
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
