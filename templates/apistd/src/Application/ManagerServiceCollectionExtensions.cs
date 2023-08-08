// 本文件由ater.dry工具自动生成.
namespace Application;

public static partial class ManagerServiceCollectionExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataStoreContext));
        services.AddScoped(typeof(BlogQueryStore));
        services.AddScoped(typeof(SystemConfigQueryStore));
        services.AddScoped(typeof(SystemMenuQueryStore));
        services.AddScoped(typeof(SystemRoleQueryStore));
        services.AddScoped(typeof(SystemUserQueryStore));
        services.AddScoped(typeof(UserQueryStore));
        services.AddScoped(typeof(BlogCommandStore));
        services.AddScoped(typeof(SystemConfigCommandStore));
        services.AddScoped(typeof(SystemMenuCommandStore));
        services.AddScoped(typeof(SystemRoleCommandStore));
        services.AddScoped(typeof(SystemUserCommandStore));
        services.AddScoped(typeof(UserCommandStore));

    }

    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped<ISystemConfigManager, SystemConfigManager>();
        services.AddScoped<ISystemMenuManager, SystemMenuManager>();
        services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        services.AddScoped<ISystemUserManager, SystemUserManager>();
        services.AddScoped<IUserManager, UserManager>();

    }
}
