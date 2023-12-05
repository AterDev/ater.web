using Microsoft.EntityFrameworkCore.Query;

namespace EntityFramework;

public partial class ContextBase : DbContext
{
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<SystemRole> SystemRoles { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }
    /// <summary>
    /// 菜单
    /// </summary>
    public DbSet<SystemMenu> SystemMenus { get; set; }
    public DbSet<SystemPermission> SystemPermissions { get; set; }
    /// <summary>
    /// 权限组
    /// </summary>
    public DbSet<SystemPermissionGroup> SystemPermissionGroups { get; set; }
    public DbSet<SystemLogs> SystemLogs { get; set; }
    public DbSet<SystemOrganization> SystemOrganizations { get; set; }
    public DbSet<User> Users { get; set; }


    public ContextBase(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {


        base.OnModelCreating(builder);
        OnModelExtendCreating(builder);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        foreach (var entityEntry in entries)
        {
            var property = entityEntry.Metadata.FindProperty("CreatedTime");
            if (property != null && property.ClrType == typeof(DateTimeOffset))
            {
                entityEntry.Property("CreatedTime").CurrentValue = DateTimeOffset.UtcNow;
            }
        }
        entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
        foreach (var entityEntry in entries)
        {
            var property = entityEntry.Metadata.FindProperty("UpdatedTime");
            if (property != null && property.ClrType == typeof(DateTimeOffset))
            {
                entityEntry.Property("UpdatedTime").CurrentValue = DateTimeOffset.UtcNow;

            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnModelExtendCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            if (typeof(IEntityBase).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.Name)
                    .HasKey("Id");
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<IEntityBase>(e => !e.IsDeleted, entityType.ClrType));
            }
        }
    }

    private static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression, Type entityType)
    {
        var newParam = Expression.Parameter(entityType);
        var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

        return Expression.Lambda(newBody, newParam);
    }
}
