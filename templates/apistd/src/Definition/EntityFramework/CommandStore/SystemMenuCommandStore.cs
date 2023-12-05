using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemMenuCommandStore : CommandSet<SystemMenu>
{
    public SystemMenuCommandStore(CommandDbContext context, ILogger<SystemMenuCommandStore> logger) : base(context, logger)
    {
    }

}
