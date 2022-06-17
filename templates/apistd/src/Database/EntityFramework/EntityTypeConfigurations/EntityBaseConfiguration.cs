using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EntityFramework.EntityTypeConfigurations;

/// <summary>
/// 基类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class EntityBaseConfiguration<Entity> : IEntityTypeConfiguration<Entity> where Entity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
    //public abstract void ConfigureOther(EntityTypeBuilder<Entity> builder);
}
