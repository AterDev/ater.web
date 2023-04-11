namespace Application.QueryStore;
public class TagsQueryStore : QuerySet<Tags>
{
    public TagsQueryStore(QueryDbContext context, ILogger<TagsQueryStore> logger) : base(context, logger)
    {
    }
}


