using Core.Entities;
using Core.Entities.SystemEntities;
namespace Application.Implement;
public class DataStoreContext
{
    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }

    public QuerySet<SystemConfig> SystemConfigQuery { get; init; }
    public QuerySet<SystemLogs> SystemLogsQuery { get; init; }
    public QuerySet<SystemMenu> SystemMenuQuery { get; init; }
    public QuerySet<SystemOrganization> SystemOrganizationQuery { get; init; }
    public QuerySet<SystemPermission> SystemPermissionQuery { get; init; }
    public QuerySet<SystemRole> SystemRoleQuery { get; init; }
    public QuerySet<SystemUser> SystemUserQuery { get; init; }
    public QuerySet<User> UserQuery { get; init; }
    public CommandSet<SystemConfig> SystemConfigCommand { get; init; }
    public CommandSet<SystemLogs> SystemLogsCommand { get; init; }
    public CommandSet<SystemMenu> SystemMenuCommand { get; init; }
    public CommandSet<SystemOrganization> SystemOrganizationCommand { get; init; }
    public CommandSet<SystemPermission> SystemPermissionCommand { get; init; }
    public CommandSet<SystemRole> SystemRoleCommand { get; init; }
    public CommandSet<SystemUser> SystemUserCommand { get; init; }
    public CommandSet<User> UserCommand { get; init; }


    /// <summary>
    /// 绑在对象
    /// </summary>
    private readonly Dictionary<string, object> SetCache = new();

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
        SystemConfigQuery = systemConfigQuery;
        AddCache(SystemConfigQuery);
        SystemLogsQuery = systemLogsQuery;
        AddCache(SystemLogsQuery);
        SystemMenuQuery = systemMenuQuery;
        AddCache(SystemMenuQuery);
        SystemOrganizationQuery = systemOrganizationQuery;
        AddCache(SystemOrganizationQuery);
        SystemPermissionQuery = systemPermissionQuery;
        AddCache(SystemPermissionQuery);
        SystemRoleQuery = systemRoleQuery;
        AddCache(SystemRoleQuery);
        SystemUserQuery = systemUserQuery;
        AddCache(SystemUserQuery);
        UserQuery = userQuery;
        AddCache(UserQuery);
        SystemConfigCommand = systemConfigCommand;
        AddCache(SystemConfigCommand);
        SystemLogsCommand = systemLogsCommand;
        AddCache(SystemLogsCommand);
        SystemMenuCommand = systemMenuCommand;
        AddCache(SystemMenuCommand);
        SystemOrganizationCommand = systemOrganizationCommand;
        AddCache(SystemOrganizationCommand);
        SystemPermissionCommand = systemPermissionCommand;
        AddCache(SystemPermissionCommand);
        SystemRoleCommand = systemRoleCommand;
        AddCache(SystemRoleCommand);
        SystemUserCommand = systemUserCommand;
        AddCache(SystemUserCommand);
        UserCommand = userCommand;
        AddCache(UserCommand);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await CommandContext.SaveChangesAsync();
    }

    public QuerySet<TEntity> QuerySet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "QueryStore";
        var set = GetSet(typename);
        if (set == null) throw new ArgumentNullException($"{typename} class object not found");
        return (QuerySet<TEntity>)set;
    }
    public CommandSet<TEntity> CommandSet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "CommandStore";
        var set = GetSet(typename);
        if (set == null) throw new ArgumentNullException($"{typename} class object not found");
        return (CommandSet<TEntity>)set;
    }

    private void AddCache(object set)
    {
        var typeName = set.GetType().Name;
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
