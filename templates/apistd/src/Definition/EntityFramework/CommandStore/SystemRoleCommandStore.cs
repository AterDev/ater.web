using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemRoleCommandStore : CommandSet<SystemRole>
{
    public SystemRoleCommandStore(CommandDbContext context, ILogger<SystemRoleCommandStore> logger) : base(context, logger)
    {
    }

}
