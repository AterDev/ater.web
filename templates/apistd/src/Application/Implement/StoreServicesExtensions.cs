namespace Application.Implement;

public static class StoreServicesExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        _ = services.AddScoped(typeof(DataStoreContext));
        _ = services.AddScoped(typeof(SystemConfigQueryStore));
        _ = services.AddScoped(typeof(SystemLogsQueryStore));
        _ = services.AddScoped(typeof(SystemMenuQueryStore));
        _ = services.AddScoped(typeof(SystemOrganizationQueryStore));
        _ = services.AddScoped(typeof(SystemPermissionQueryStore));
        _ = services.AddScoped(typeof(SystemRoleQueryStore));
        _ = services.AddScoped(typeof(SystemUserQueryStore));
        _ = services.AddScoped(typeof(UserQueryStore));
        _ = services.AddScoped(typeof(SystemConfigCommandStore));
        _ = services.AddScoped(typeof(SystemLogsCommandStore));
        _ = services.AddScoped(typeof(SystemMenuCommandStore));
        _ = services.AddScoped(typeof(SystemOrganizationCommandStore));
        _ = services.AddScoped(typeof(SystemPermissionCommandStore));
        _ = services.AddScoped(typeof(SystemRoleCommandStore));
        _ = services.AddScoped(typeof(SystemUserCommandStore));
        _ = services.AddScoped(typeof(UserCommandStore));

    }

    public static void AddManager(this IServiceCollection services)
    {
        _ = services.AddHttpContextAccessor();
        _ = services.AddTransient<IUserContext, UserContext>();
        _ = services.AddScoped<ISystemConfigManager, SystemConfigManager>();
        _ = services.AddScoped<ISystemLogsManager, SystemLogsManager>();
        _ = services.AddScoped<ISystemMenuManager, SystemMenuManager>();
        _ = services.AddScoped<ISystemOrganizationManager, SystemOrganizationManager>();
        _ = services.AddScoped<ISystemPermissionManager, SystemPermissionManager>();
        _ = services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        _ = services.AddScoped<ISystemUserManager, SystemUserManager>();
        _ = services.AddScoped<IUserManager, UserManager>();

    }
}
