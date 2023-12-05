using Entity.CMS;

namespace EntityFramework.QueryStore;
public class CatalogQueryStore : QuerySet<Catalog>
{
    public CatalogQueryStore(QueryDbContext context, ILogger<CatalogQueryStore> logger) : base(context, logger)
    {
    }
}


