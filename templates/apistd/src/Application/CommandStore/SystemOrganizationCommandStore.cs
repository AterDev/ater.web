namespace Application.CommandStore;
public class SystemOrganizationCommandStore : CommandSet<SystemOrganization>
{
    public SystemOrganizationCommandStore(CommandDbContext context, ILogger<SystemOrganizationCommandStore> logger) : base(context, logger)
    {
    }

}
