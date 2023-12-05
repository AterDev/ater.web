using Entity.CMS;

namespace EntityFramework.CommandStore;
public class CatalogCommandStore : CommandSet<Catalog>
{
    public CatalogCommandStore(CommandDbContext context, ILogger<CatalogCommandStore> logger) : base(context, logger)
    {
    }

}
