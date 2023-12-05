using Entity.CMS;

namespace EntityFramework.QueryStore;
public class BlogQueryStore : QuerySet<Blog>
{
    public BlogQueryStore(QueryDbContext context, ILogger<BlogQueryStore> logger) : base(context, logger)
    {
    }
}

