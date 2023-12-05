using Entity.System;

namespace EntityFramework.CommandStore;
public class SystemPermissionGroupCommandStore : CommandSet<SystemPermissionGroup>
{
    public SystemPermissionGroupCommandStore(CommandDbContext context, ILogger<SystemPermissionGroupCommandStore> logger) : base(context, logger)
    {
    }

}
