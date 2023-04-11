namespace Application.QueryStore;
public class RolePermissionQueryStore : QuerySet<RolePermission>
{
    public RolePermissionQueryStore(QueryDbContext context, ILogger<RolePermissionQueryStore> logger) : base(context, logger)
    {
    }
}


