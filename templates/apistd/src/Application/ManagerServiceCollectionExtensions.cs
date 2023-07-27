// 本文件由ater.dry工具自动生成.

using Application.Manager;
using EntityFramework.CommandStore;
using EntityFramework.QueryStore;

namespace Application;

/// <summary>
/// 业务数据服务注入扩展
/// </summary>
public static partial class ManagerServiceCollectionExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataStoreContext));
        services.AddScoped(typeof(BlogQueryStore));
        services.AddScoped(typeof(SystemRoleQueryStore));
        services.AddScoped(typeof(SystemUserQueryStore));
        services.AddScoped(typeof(BlogCommandStore));
        services.AddScoped(typeof(SystemRoleCommandStore));
        services.AddScoped(typeof(SystemUserCommandStore));
    }

    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        services.AddScoped<ISystemUserManager, SystemUserManager>();
    }
}
