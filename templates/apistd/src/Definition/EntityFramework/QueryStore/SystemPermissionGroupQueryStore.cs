using Entity.System;

namespace EntityFramework.QueryStore;
public class SystemPermissionGroupQueryStore : QuerySet<SystemPermissionGroup>
{
    public SystemPermissionGroupQueryStore(QueryDbContext context, ILogger<SystemPermissionGroupQueryStore> logger) : base(context, logger)
    {
    }
}


