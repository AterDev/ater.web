namespace Application.CommandStore;
public class CatalogCommandStore : CommandSet<Catalog>
{
    public CatalogCommandStore(CommandDbContext context, ILogger<CatalogCommandStore> logger) : base(context, logger)
    {
    }

}
