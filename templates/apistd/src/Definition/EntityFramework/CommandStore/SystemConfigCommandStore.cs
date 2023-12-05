using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemConfigCommandStore : CommandSet<SystemConfig>
{
    public SystemConfigCommandStore(CommandDbContext context, ILogger<SystemConfigCommandStore> logger) : base(context, logger)
    {
    }

}
