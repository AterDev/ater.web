using Application.Interface;

namespace Application.Implement;

public static class StoreServicesExtensions
{
    public static void AddDataStore(this IServiceCollection services)
    {
        services.AddTransient<IUserContext, UserContext>();
        services.AddScoped(typeof(DataStoreContext));

    }

    public static void AddManager(this IServiceCollection services)
    {

    }
}
