namespace EntityFramework;
public class CommandDbContext : ContextBase
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.Entity<IEntityBase>().HasQueryFilter(e => !e.IsDeleted);
        base.OnModelCreating(builder);
    }

}
