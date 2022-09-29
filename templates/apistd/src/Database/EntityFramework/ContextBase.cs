using Core.Entities;
using Core.Models;

namespace EntityFramework;

public class ContextBase : DbContext
{
    public DbSet<SystemUser> Users { get; set; }
    public DbSet<SystemRole> Roles { get; set; }

    public ContextBase(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<EntityBase>().UseTpcMappingStrategy();
        builder.Entity<SystemUser>(e =>
        {
            e.HasIndex(a => a.Email);
            e.HasIndex(a => a.PhoneNumber);
            e.HasIndex(a => a.UserName);
            e.HasIndex(a => a.IsDeleted);
            e.HasIndex(a => a.CreatedTime);
        });

        builder.Entity<SystemRole>(e =>
        {
            e.HasIndex(m => m.Name);
        });

        base.OnModelCreating(builder);
    }
}
