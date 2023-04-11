namespace Application.CommandStore;
public class RolePermissionCommandStore : CommandSet<RolePermission>
{
    public RolePermissionCommandStore(CommandDbContext context, ILogger<RolePermissionCommandStore> logger) : base(context, logger)
    {
    }

}
