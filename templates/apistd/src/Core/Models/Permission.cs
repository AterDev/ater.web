namespace Core.Models;
/// <summary>
/// 权限
/// </summary>
public class Permission : EntityBase
{
    [MaxLength(30)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 父级权限
    /// </summary>
    public Permission? Parent { get; set; }
    /// <summary>
    /// 权限路径
    /// </summary>
    [MaxLength(200)]
    public string? PermissionPath { get; set; }
    public List<Role>? Roles { get; set; }
    public List<RolePermission>? RolePermissions { get; set; }


}
