// 本文件由 ater.dry工具自动生成.
using Application.Managers;

namespace Application;

public static partial class ManagerServiceCollectionExtensions
{
    public static void AddManager(this IServiceCollection services)
    {
        services.AddScoped(typeof(DataAccessContext<>));
        services.AddScoped(typeof(UserManager));
    }
}
