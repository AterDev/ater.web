namespace Share.Models.SystemOrganizationDtos;
/// <summary>
/// 组织结构更新时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemOrganization"/>
public class SystemOrganizationUpdateDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    public Guid? ParentId { get; set; }

}
