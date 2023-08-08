using Entity.SystemEntities;
namespace Share.Models.SystemPermissionDtos;
/// <summary>
/// 权限查询筛选
/// </summary>
/// <see cref="Entity.SystemEntities.SystemPermission"/>
public class SystemPermissionFilterDto : FilterBase
{
    /// <summary>
    /// 权限名称标识
    /// </summary>
    [MaxLength(30)]
    public string? Name { get; set; }
    /// <summary>
    /// 权限说明
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enable { get; set; }
    /// <summary>
    /// 权限类型
    /// </summary>
    public PermissionType? PermissionType { get; set; }
    public Guid? GroupId { get; set; }
    
}
