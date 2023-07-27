using Entity.CMSEntities;

namespace EntityFramework.QueryStore;
public class BlogQueryStore : QuerySet<Blog>
{
    public BlogQueryStore(QueryDbContext context, ILogger<BlogQueryStore> logger) : base(context, logger)
    {
    }
}

