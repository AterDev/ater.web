namespace Application.QueryStore;
public class RoleQueryStore : QuerySet<Role>
{
    public RoleQueryStore(QueryDbContext context, ILogger<RoleQueryStore> logger) : base(context, logger)
    {
    }
}


