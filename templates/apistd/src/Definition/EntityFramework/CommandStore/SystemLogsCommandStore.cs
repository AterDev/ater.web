using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemLogsCommandStore : CommandSet<SystemLogs>
{
    public SystemLogsCommandStore(CommandDbContext context, ILogger<SystemLogsCommandStore> logger) : base(context, logger)
    {
    }

}
