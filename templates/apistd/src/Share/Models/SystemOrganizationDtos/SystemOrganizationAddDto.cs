using Core.Entities.SystemEntities;
namespace Share.Models.SystemOrganizationDtos;
/// <summary>
/// 组织结构添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemOrganization"/>
public class SystemOrganizationAddDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }
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
