namespace Core.Entities;
/// <summary>
/// 角色权限中间表
/// </summary>
public class RolePermission : EntityBase
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType PermissionTypeMyProperty { get; set; } = PermissionType.Write;

    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;


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