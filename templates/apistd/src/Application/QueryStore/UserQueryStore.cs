namespace Application.QueryStore;
public class UserQueryStore : QuerySet<User>
{
    public UserQueryStore(QueryDbContext context, ILogger<UserQueryStore> logger) : base(context, logger)
    {
    }
}


