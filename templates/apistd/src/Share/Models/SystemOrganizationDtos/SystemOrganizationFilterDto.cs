namespace Share.Models.SystemOrganizationDtos;
/// <summary>
/// 组织结构查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemOrganization"/>
public class SystemOrganizationFilterDto : FilterBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public string? Name { get; set; }
    public Guid? ParentId { get; set; }

}
