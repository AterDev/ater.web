namespace Application.CommandStore;
public class SystemMenuCommandStore : CommandSet<SystemMenu>
{
    public SystemMenuCommandStore(CommandDbContext context, ILogger<SystemMenuCommandStore> logger) : base(context, logger)
    {
    }

}
