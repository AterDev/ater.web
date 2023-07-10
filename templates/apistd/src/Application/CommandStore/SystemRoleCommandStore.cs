using Entity.SystemEntities;

namespace Application.CommandStore;
public class SystemRoleCommandStore : CommandSet<SystemRole>
{
    public SystemRoleCommandStore(CommandDbContext context, ILogger<SystemRoleCommandStore> logger) : base(context, logger)
    {
    }

}
