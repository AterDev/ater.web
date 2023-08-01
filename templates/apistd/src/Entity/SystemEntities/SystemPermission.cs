namespace Entity.SystemEntities;
/// <summary>
/// 权限
/// </summary>
public class SystemPermission : EntityBase
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

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType PermissionType { get; set; }

    /// <summary>
    /// 权限组
    /// </summary>
    public required SystemPermissionGroup Group { get; set; }
}

/// <summary>
/// 权限类型
/// </summary>
public enum PermissionType
{
    Read = 0,
    /// <summary>
    /// 审核
    /// </summary>
    Audit = 1,
    Add,
    Edit,
    /// <summary>
    /// 可读写
    /// </summary>
    Write,
    AuditWrite
}
