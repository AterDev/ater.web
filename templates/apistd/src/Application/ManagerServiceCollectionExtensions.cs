// 本文件由 ater.dry工具自动生成.
namespace Application;

public static partial class ManagerServiceCollectionExtensions
{
    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataAccessContext<>));
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
