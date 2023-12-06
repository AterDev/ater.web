using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Caching.Distributed;

namespace EntityFramework.DBProvider;
public class CommandDbContextFactory : IDesignTimeDbContextFactory<CommandDbContext>
{
    private readonly ITenantProvider _tenantProvider;
    private readonly IDistributedCache _cache;

    public CommandDbContextFactory(ITenantProvider tenantProvider, IDistributedCache cache)
    {
        _tenantProvider = tenantProvider;
        _cache = cache;
    }

    public CommandDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CommandDbContext>();
        var tenantId = _tenantProvider.TenantId;

        // 从缓存或配置中查询连接字符串
        var connectionStrings = _cache.GetString($"{tenantId}_CommandConnectionString");

        optionsBuilder.UseNpgsql(connectionStrings);
        return new CommandDbContext(optionsBuilder.Options);
    }
}
