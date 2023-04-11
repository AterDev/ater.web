using Core.Entities.SystemEntities;
namespace Share.Models.SystemPermissionDtos;
/// <summary>
/// 权限添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemPermission"/>
public class SystemPermissionAddDto
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
    public Guid ParentId { get; set; }
    
}
