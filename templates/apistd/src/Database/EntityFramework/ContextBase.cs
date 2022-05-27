namespace EntityFramework;

public class ContextBase : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    public ContextBase(DbContextOptions<ContextBase> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // 默认配置
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(e =>
        {
            e.HasIndex(a => a.Email);
            e.HasIndex(a => a.PhoneNumber);
            e.HasIndex(a => a.UserName);
            e.HasIndex(a => a.IsDeleted);
            e.HasIndex(a => a.CreatedTime);
            e.HasIndex(a => a.Status);

        });

        builder.Entity<Role>(e =>
        {
            e.HasIndex(m => m.Name);
            e.HasIndex(m => m.Status);
        });

        base.OnModelCreating(builder);
    }
}
