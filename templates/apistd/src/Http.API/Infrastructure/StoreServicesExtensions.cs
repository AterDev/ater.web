using Application.CommandStore;
using Application.Manager;
using Application.QueryStore;

namespace Http.API.Infrastructure;

public static partial class StoreServicesExtensions
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
        services.AddTransient<IUserContext, UserContext>();
        services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        services.AddScoped<ISystemUserManager, SystemUserManager>();
    }
}
