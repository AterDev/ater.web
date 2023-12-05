using Entity.FileManager;

namespace EntityFramework.QueryStore;
public class FileDataQueryStore : QuerySet<FileData>
{
    public FileDataQueryStore(QueryDbContext context, ILogger<FileDataQueryStore> logger) : base(context, logger)
    {
    }
}
