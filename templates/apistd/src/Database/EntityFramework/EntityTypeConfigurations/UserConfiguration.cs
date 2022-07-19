using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EntityFramework.EntityTypeConfigurations;
internal class UserConfiguration : EntityBaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        // 设置comment ，index等内容
        builder.HasComment("用户名").HasIndex(x => x.UserName);
    }
}
