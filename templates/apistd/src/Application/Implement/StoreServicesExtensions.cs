namespace Application.Implement;

public static class StoreServicesExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddTransient<IUserContext, UserContext>();
        services.AddScoped(typeof(DataStoreContext));
        services.AddScoped(typeof(RoleQueryStore));
        services.AddScoped(typeof(UserQueryStore));
        services.AddScoped(typeof(RoleCommandStore));
        services.AddScoped(typeof(UserCommandStore));

    }

    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped<IRoleManager, RoleManager>();
        services.AddScoped<IUserManager, UserManager>();
    }
}
