namespace Entity.SystemMod;

/// <summary>
/// 系统权限组
/// </summary>
[Module(Modules.System)]
[Index(nameof(Name))]
public class SystemPermissionGroup : IEntityBase
{
    /// <summary>
    /// 权限名称标识
    /// </summary>
    [MaxLength(30)]
    public required string Name { get; set; }
    /// <summary>
    /// 权限说明
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    public ICollection<SystemPermission> Permissions { get; set; } = [];

    public ICollection<SystemRole> Roles { get; set; } = [];
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}