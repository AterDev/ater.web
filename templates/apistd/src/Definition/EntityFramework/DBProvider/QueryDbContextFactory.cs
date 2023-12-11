using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Distributed;

namespace EntityFramework.DBProvider;
public class QueryDbContextFactory(ITenantProvider tenantProvider, IDistributedCache cache) : IDesignTimeDbContextFactory<QueryDbContext>
{
    private readonly ITenantProvider _tenantProvider = tenantProvider;
    private readonly IDistributedCache _cache = cache;

    public QueryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<QueryDbContext>();
        Guid tenantId = _tenantProvider.TenantId;

        // 从缓存或配置中查询连接字符串
        var connectionStrings = _cache.GetString($"{tenantId}_QueryConnectionString");

        optionsBuilder.UseNpgsql(connectionStrings);
        return new QueryDbContext(optionsBuilder.Options);
    }
}
