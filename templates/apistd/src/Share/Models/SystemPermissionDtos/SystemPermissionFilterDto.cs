namespace Share.Models.SystemPermissionDtos;
/// <summary>
/// 权限查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemPermission"/>
public class SystemPermissionFilterDto : FilterBase
{
    [MaxLength(30)]
    public string? Name { get; set; }
    /// <summary>
    /// 权限路径
    /// </summary>
    [MaxLength(200)]
    public string? PermissionPath { get; set; }
    public Guid? ParentId { get; set; }

}
