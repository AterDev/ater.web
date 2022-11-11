namespace Application.Implement;

public static class StoreServicesExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        _ = services.AddTransient<IUserContext, UserContext>();
        _ = services.AddScoped(typeof(DataStoreContext));
        _ = services.AddScoped(typeof(SystemRoleQueryStore));
        _ = services.AddScoped(typeof(SystemUserQueryStore));
        _ = services.AddScoped(typeof(SystemRoleCommandStore));
        _ = services.AddScoped(typeof(SystemUserCommandStore));

    }

    public static void AddManager(this IServiceCollection services)
    {
        _ = services.AddScoped<ISystemRoleManager, SystemRoleManager>();
        _ = services.AddScoped<ISystemUserManager, SystemUserManager>();

    }
}
