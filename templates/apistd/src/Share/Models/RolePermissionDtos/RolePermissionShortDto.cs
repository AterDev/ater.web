using Core.Entities.SystemEntities;
namespace Share.Models.RolePermissionDtos;
/// <summary>
/// 角色权限中间概要
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.RolePermission"/>
public class RolePermissionShortDto
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType PermissionTypeMyProperty { get; set; } = PermissionType.Write;
    public SystemRole Role { get; set; } = null!;
    public SystemPermission Permission { get; set; } = null!;
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
