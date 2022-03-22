namespace Core.Models;

/// <summary>
/// 目录/文件目录 / 自引用
/// </summary>
public partial class LibraryCatalog : EntityBase
{
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(50)]
    public string? Type { get; set; }
    public short Sort { get; set; } = 0;
    public short Level { get; set; } = 0;
    [ForeignKey("ParentId")]
    public LibraryCatalog? Parent { get; set; }
    public Guid? ParentId { get; set; }
    /// <summary>
    /// 所属用户
    /// </summary>
    [ForeignKey("AccountId")]
    public User Account { get; set; } = null!;
    public Guid AccountId { get; set; }
    /// <summary>
    /// 子目录
    /// </summary>
    public List<LibraryCatalog>? Catalogs { get; set; }
}
