// ���ļ���ater.dry�����Զ�����.

using Application.Manager;
using EntityFramework.CommandStore;
using EntityFramework.QueryStore;

namespace Application;

/// <summary>
/// ҵ�����ݷ���ע����չ
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
