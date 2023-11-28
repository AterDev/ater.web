
namespace EntityFramework;
/// <summary>
/// 只读数据库上下文
/// </summary>
public class QueryDbContext : ContextBase
{

    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("read-only context can't save data");
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("read-only context can't save data");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 全局过滤
        _ = builder.Entity<IEntityBase>().HasQueryFilter(e => !e.IsDeleted);
        base.OnModelCreating(builder);
    }
}
