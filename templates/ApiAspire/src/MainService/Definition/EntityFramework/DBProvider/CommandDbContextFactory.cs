﻿using Microsoft.Extensions.Caching.Distributed;

namespace Definition.EntityFramework.DBProvider;
public class CommandDbContextFactory(ITenantProvider tenantProvider, IDistributedCache cache) : IDbContextFactory<CommandDbContext>
{
    private readonly ITenantProvider _tenantProvider = tenantProvider;
    private readonly IDistributedCache _cache = cache;

    public CommandDbContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<CommandDbContext>();
        Guid tenantId = _tenantProvider.TenantId;

        // 从缓存或配置中查询连接字符串
        var connectionStrings = _cache.GetString($"{tenantId}_CommandConnectionString");

        builder.UseNpgsql(connectionStrings);
        return new CommandDbContext(builder.Options);
    }
}
