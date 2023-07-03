namespace EntityFramework;
public class CmsCommandDb : ContextBase
{
    public CmsCommandDb(DbContextOptions<CmsCommandDb> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.Entity<EntityBase>().HasQueryFilter(e => !e.IsDeleted);
        base.OnModelCreating(builder);
    }

}
