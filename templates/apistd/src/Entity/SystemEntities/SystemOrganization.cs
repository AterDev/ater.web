using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.SystemEntities;
/// <summary>
/// 组织结构
/// </summary>
public class SystemOrganization : EntityBase, ITreeNode<SystemOrganization>
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
    [ForeignKey(nameof(ParentId))]
    public SystemOrganization? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public List<SystemUser>? Users { get; set; }
}
