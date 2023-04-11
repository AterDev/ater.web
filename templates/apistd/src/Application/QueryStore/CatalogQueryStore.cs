namespace Application.QueryStore;
public class CatalogQueryStore : QuerySet<Catalog>
{
    public CatalogQueryStore(QueryDbContext context, ILogger<CatalogQueryStore> logger) : base(context, logger)
    {
    }
}


