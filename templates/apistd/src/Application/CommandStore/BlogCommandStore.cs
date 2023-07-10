using Application.Implement;

namespace Application.CommandStore;
public class BlogCommandStore : CommandSet<Blog>
{
    public BlogCommandStore(CommandDbContext context, ILogger<BlogCommandStore> logger) : base(context, logger)
    {
    }

}
