using Application.CommandStore;
using Application.Manager;
using Application.QueryStore;

namespace Grpc.API;

/// <summary>
/// 业务数据服务注入扩展
/// </summary>
public static partial class StoreServicesExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataStoreContext));
        services.AddScoped(typeof(SystemRoleQueryStore));
        services.AddScoped(typeof(SystemUserQueryStore));
        services.AddScoped(typeof(SystemRoleCommandStore));
        services.AddScoped(typeof(SystemUserCommandStore));
    }

    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        services.AddScoped<ISystemUserManager, SystemUserManager>();
    }
}
