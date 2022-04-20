namespace Core.Models;
/// <summary>
/// 角色表
/// </summary>
public class Role : EntityBase
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [MaxLength(30)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }

    public List<User>? Users { get; set; }
    public List<Permission>? Permissions { get; set; }
    public List<RolePermission>? RolePermissions { get; set; }

}
