using Entity.System;

namespace EntityFramework.QueryStore;
public class SystemMenuQueryStore : QuerySet<SystemMenu>
{
    public SystemMenuQueryStore(QueryDbContext context, ILogger<SystemMenuQueryStore> logger) : base(context, logger)
    {
    }
}


