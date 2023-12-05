using Entity.FileManager;
namespace EntityFramework.QueryStore;
public class FolderQueryStore : QuerySet<Folder>
{
    public FolderQueryStore(QueryDbContext context, ILogger<FolderQueryStore> logger) : base(context, logger)
    {
    }
}
