using Microsoft.AspNetCore.Identity;

namespace Core.Identity;

/// <summary>
/// 角色表
/// </summary>
public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
    public override string Name { get; set; } = null!;
    public override string? NormalizedName { get; set; }
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }
    public virtual Status Status { get; set; } = Status.Default;
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
}
