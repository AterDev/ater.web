namespace Application.CommandStore;
public class SystemConfigCommandStore : CommandSet<SystemConfig>
{
    public SystemConfigCommandStore(CommandDbContext context, ILogger<SystemConfigCommandStore> logger) : base(context, logger)
    {
    }

}
