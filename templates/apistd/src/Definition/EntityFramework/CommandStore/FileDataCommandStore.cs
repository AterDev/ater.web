using Entity.FileManager;
namespace EntityFramework.CommandStore;
public class FileDataCommandStore : CommandSet<FileData>
{
    public FileDataCommandStore(CommandDbContext context, ILogger<FileDataCommandStore> logger) : base(context, logger)
    {
    }

}
