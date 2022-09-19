namespace Application.CommandStore;
public class RoleCommandStore : CommandSet<Role>
{
    public RoleCommandStore(CommandDbContext context, ILogger<RoleCommandStore> logger) : base(context, logger)
    {
    }

}
