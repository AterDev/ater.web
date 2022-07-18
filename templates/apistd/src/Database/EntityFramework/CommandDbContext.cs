namespace EntityFramework;
public class CommandDbContext : ContextBase
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
