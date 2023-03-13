using Core.Entities;
using Core.Models;

namespace EntityFramework;

public class ContextBase : DbContext
{
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<WebConfig> WebConfigs { get; set; }
    public DbSet<SystemRole> SystemRoles { get; set; }
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
