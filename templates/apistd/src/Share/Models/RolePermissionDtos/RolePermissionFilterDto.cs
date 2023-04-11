using Core.Entities.SystemEntities;
namespace Share.Models.RolePermissionDtos;
/// <summary>
/// 角色权限中间查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.RolePermission"/>
public class RolePermissionFilterDto : FilterBase
{
    public Guid? RoleId { get; set; }
    public Guid? PermissionId { get; set; }
    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType? PermissionTypeMyProperty { get; set; }
    
}
