using Microsoft.Extensions.Configuration;

namespace Application;

/// <summary>
/// 应用配置常量
/// </summary>
public static class AppSetting
{
    public const string None = "none";
    public const string PostgreSQL = "postgresql";
    public const string SQLServer = "sqlserver";
    public const string MySQL = "mysql";
    public const string Redis = "redis";
    public const string Memory = "memory";
    public const string Otlp = "otlp";

    public const string CommandDB = "CommandDb";
    public const string QueryDB = "QueryDb";
    public const string Cache = "Cache";
    public const string CacheInstanceName = "CacheInstanceName";
    public const string Logging = "Logging";
}

/// <summary>
/// 服务注册扩展
/// </summary>
public static partial class AppServiceCollectionExtensions
{
    /// <returns></returns>
    public static IServiceCollection AddAppComponents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPgsqlDbContext(configuration);
        services.AddRedisCache(configuration);
        return services;
    }

    /// <summary>
    /// add postgresql config
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPgsqlDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var commandString = configuration.GetConnectionString(AppSetting.CommandDB);
        var queryString = configuration.GetConnectionString(AppSetting.QueryDB);
        services.AddDbContextPool<QueryDbContext>(option =>
        {
            option.UseNpgsql(queryString, sql =>
            {
                sql.MigrationsAssembly("Http.API");
                sql.CommandTimeout(10);
            });
        });
        services.AddDbContextPool<CommandDbContext>(option =>
        {
            option.UseNpgsql(commandString, sql =>
            {
                sql.MigrationsAssembly("Http.API");
                sql.CommandTimeout(10);
            });
        });
        return services;
    }

    /// <summary>
    /// add redis cache config
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString(AppSetting.Cache);
            options.InstanceName = configuration.GetConnectionString(AppSetting.CacheInstanceName);
        });
    }
}
