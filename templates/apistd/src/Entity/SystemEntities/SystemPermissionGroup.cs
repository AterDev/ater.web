namespace Entity.SystemEntities;

public class SystemPermissionGroup : EntityBase
{
    /// <summary>
    /// 权限名称标识
    /// </summary>
    [MaxLength(30)]
    public required string Name { get; set; }
    /// <summary>
    /// 权限说明
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    public ICollection<SystemPermission> Permissions { get; set; } = new List<SystemPermission>();

    public ICollection<SystemRole> Roles{ get; set; } = new List<SystemRole>();
}