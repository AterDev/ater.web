namespace EntityFramework;

public class ContextBase : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<User> Users { get; set; }

    public ContextBase(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.Entity<EntityBase>().UseTpcMappingStrategy();
        base.OnModelCreating(builder);
    }
}
