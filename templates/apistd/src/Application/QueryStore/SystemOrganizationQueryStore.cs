namespace Application.QueryStore;
public class SystemOrganizationQueryStore : QuerySet<SystemOrganization>
{
    public SystemOrganizationQueryStore(QueryDbContext context, ILogger<SystemOrganizationQueryStore> logger) : base(context, logger)
    {
    }
}


