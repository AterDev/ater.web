using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemUserCommandStore : CommandSet<SystemUser>
{
    public SystemUserCommandStore(CommandDbContext context, ILogger<SystemUserCommandStore> logger) : base(context, logger)
    {
    }

}
