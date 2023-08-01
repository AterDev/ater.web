namespace EntityFramework;

public partial class ContextBase : DbContext
{
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<SystemRole> SystemRoles { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }
    public DbSet<SystemMenu> SystemMenus { get; set; }
    public DbSet<SystemPermission> SystemPermissions { get; set; }
    public DbSet<SystemPermissionGroup> SystemPermissionGroups { get; set; }
    public DbSet<SystemLogs> SystemLogs { get; set; }
    public DbSet<SystemOrganization> SystemOrganizations { get; set; }
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
