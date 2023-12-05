using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.FileManager;

/// <summary>
/// 文件夹
/// </summary>
[Index(nameof(Name))]
[Module(Modules.FileManager)]
public class Folder : IEntityBase, ITreeNode<Folder>
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    public Folder? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public List<Folder> Children { get; set; } = new List<Folder>();

    /// <summary>
    /// 路径
    /// </summary>
    [Column(TypeName = "ltree")]
    [MaxLength(500)]
    public string? Path { get; set; }

    public ICollection<FileData> Files { get; set; } = new List<FileData>();
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}
