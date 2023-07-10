using Application.Implement;
using Entity.SystemEntities;

namespace Application.QueryStore;
public class SystemUserQueryStore : QuerySet<SystemUser>
{
    public SystemUserQueryStore(QueryDbContext context, ILogger<SystemUserQueryStore> logger) : base(context, logger)
    {
    }
}

