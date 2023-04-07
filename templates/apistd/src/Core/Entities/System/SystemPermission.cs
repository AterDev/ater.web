namespace Core.Entities.System;
/// <summary>
/// 权限
/// </summary>
public class SystemPermission : EntityBase
{
    [MaxLength(30)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 父级权限
    /// </summary>
    public SystemPermission? Parent { get; set; }
    /// <summary>
    /// 权限路径
    /// </summary>
    [MaxLength(200)]
    public string? PermissionPath { get; set; }
    public List<SystemRole>? Roles { get; set; }
    public List<RolePermission>? RolePermissions { get; set; }
}
