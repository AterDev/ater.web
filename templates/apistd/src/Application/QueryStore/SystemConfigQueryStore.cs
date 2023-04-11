namespace Application.QueryStore;
public class SystemConfigQueryStore : QuerySet<SystemConfig>
{
    public SystemConfigQueryStore(QueryDbContext context, ILogger<SystemConfigQueryStore> logger) : base(context, logger)
    {
    }
}


