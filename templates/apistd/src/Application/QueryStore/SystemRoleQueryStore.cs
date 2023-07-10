using Application.Implement;
using Entity.SystemEntities;

namespace Application.QueryStore;
public class SystemRoleQueryStore : QuerySet<SystemRole>
{
    public SystemRoleQueryStore(QueryDbContext context, ILogger<SystemRoleQueryStore> logger) : base(context, logger)
    {
    }
}
