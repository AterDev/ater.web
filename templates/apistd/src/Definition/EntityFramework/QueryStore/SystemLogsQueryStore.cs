
using Entity.System;

namespace EntityFramework.QueryStore;
public class SystemLogsQueryStore : QuerySet<SystemLogs>
{
    public SystemLogsQueryStore(QueryDbContext context, ILogger<SystemLogsQueryStore> logger) : base(context, logger)
    {
    }
}
