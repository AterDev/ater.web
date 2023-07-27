using Entity.SystemEntities;

namespace EntityFramework.QueryStore;
public class SystemUserQueryStore : QuerySet<SystemUser>
{
    public SystemUserQueryStore(QueryDbContext context, ILogger<SystemUserQueryStore> logger) : base(context, logger)
    {
    }
}

