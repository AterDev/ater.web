namespace Application.CommandStore;
public class SystemPermissionCommandStore : CommandSet<SystemPermission>
{
    public SystemPermissionCommandStore(CommandDbContext context, ILogger<SystemPermissionCommandStore> logger) : base(context, logger)
    {
    }

}
