using Core.Entities.SystemEntities;
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
    /// <summary>
    /// 子目录
    /// </summary>
    public List<SystemOrganization>? Children { get; set; }
    /// <summary>
    /// 父目录
    /// </summary>
    public SystemOrganization? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public List<SystemUser>? Users { get; set; }
    
}
