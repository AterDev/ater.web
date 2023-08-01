namespace EntityFramework.CommandStore;
public class UserCommandStore : CommandSet<User>
{
    public UserCommandStore(CommandDbContext context, ILogger<UserCommandStore> logger) : base(context, logger)
    {
    }

}
