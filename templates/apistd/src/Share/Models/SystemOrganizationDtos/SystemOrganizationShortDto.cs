using Core.Entities.SystemEntities;
namespace Share.Models.SystemOrganizationDtos;
/// <summary>
/// 组织结构概要
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemOrganization"/>
public class SystemOrganizationShortDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 父目录
    /// </summary>
    public SystemOrganization? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;

}
