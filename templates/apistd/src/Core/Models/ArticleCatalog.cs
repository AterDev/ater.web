namespace Core.Models;

[Table("ArticleCatalog")]
public class ArticleCatalog : EntityBase
{
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(50)]
    public string? Type { get; set; }
    public short Sort { get; set; } = 0;
    public short Level { get; set; } = 0;
    /// <summary>
    /// 该目录的文章
    /// </summary>
    public List<Article>? Articles { get; set; }
    /// <summary>
    /// 父目录
    /// </summary>
    [ForeignKey("ParentId")]
    public ArticleCatalog? Parent { get; set; }
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
    public List<ArticleCatalog>? Catalogs { get; set; }

}
