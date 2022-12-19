using Core.Entities;
using Core.Models;

namespace EntityFramework;

public class ContextBase : DbContext
{
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<WebConfig> WebConfigs { get; set; }
    public DbSet<SystemRole> SystemRoles { get; set; }

    public ContextBase(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.Entity<EntityBase>().UseTpcMappingStrategy();
        _ = builder.Entity<SystemUser>(e =>
        {
            _ = e.HasIndex(a => a.Email);
            _ = e.HasIndex(a => a.PhoneNumber);
            _ = e.HasIndex(a => a.UserName);
            _ = e.HasIndex(a => a.IsDeleted);
            _ = e.HasIndex(a => a.CreatedTime);
        });

        _ = builder.Entity<SystemRole>(e =>
        {
            _ = e.HasIndex(m => m.Name);
        });

        base.OnModelCreating(builder);
    }
}
