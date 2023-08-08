namespace EntityFramework.QueryStore;
public class SystemPermissionQueryStore : QuerySet<SystemPermission>
{
    public SystemPermissionQueryStore(QueryDbContext context, ILogger<SystemPermissionQueryStore> logger) : base(context, logger)
    {
    }
}


