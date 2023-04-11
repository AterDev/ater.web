namespace Application.QueryStore;
public class BlogQueryStore : QuerySet<Blog>
{
    public BlogQueryStore(QueryDbContext context, ILogger<BlogQueryStore> logger) : base(context, logger)
    {
    }
}


