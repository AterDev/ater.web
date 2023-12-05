using Entity.FileManager;
namespace EntityFramework.CommandStore;
public class FolderCommandStore : CommandSet<Folder>
{
    public FolderCommandStore(CommandDbContext context, ILogger<FolderCommandStore> logger) : base(context, logger)
    {
    }

}
