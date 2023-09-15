// 本文件由 ater.dry工具自动生成.
namespace Application;

public static partial class ManagerServiceCollectionExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataStoreContext));
        services.AddScoped(typeof(BlogQueryStore));
        services.AddScoped(typeof(SystemConfigQueryStore));
        services.AddScoped(typeof(SystemLogsQueryStore));
        services.AddScoped(typeof(SystemMenuQueryStore));
        services.AddScoped(typeof(SystemPermissionGroupQueryStore));
        services.AddScoped(typeof(SystemPermissionQueryStore));
        services.AddScoped(typeof(SystemRoleQueryStore));
        services.AddScoped(typeof(SystemUserQueryStore));
        services.AddScoped(typeof(UserQueryStore));
        services.AddScoped(typeof(BlogCommandStore));
        services.AddScoped(typeof(SystemConfigCommandStore));
        services.AddScoped(typeof(SystemLogsCommandStore));
        services.AddScoped(typeof(SystemMenuCommandStore));
        services.AddScoped(typeof(SystemPermissionCommandStore));
        services.AddScoped(typeof(SystemPermissionGroupCommandStore));
        services.AddScoped(typeof(SystemRoleCommandStore));
        services.AddScoped(typeof(SystemUserCommandStore));
        services.AddScoped(typeof(UserCommandStore));

    }

    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped(typeof(SystemConfigManager));
        services.AddScoped(typeof(SystemLogsManager));
        services.AddScoped(typeof(SystemMenuManager));
        services.AddScoped(typeof(SystemPermissionGroupManager));
        services.AddScoped(typeof(SystemPermissionManager));
        services.AddScoped(typeof(SystemRoleManager));
        services.AddScoped(typeof(SystemUserManager));
        services.AddScoped(typeof(UserManager));

    }
}
