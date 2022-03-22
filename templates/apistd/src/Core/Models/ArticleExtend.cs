namespace Core.Models;

/// <summary>
/// 文章扩展
/// </summary>
public class ArticleExtend : EntityBase
{
    public Article Article { get; set; } = null!;
    public Guid ArticleId { get; set; }
    public string Content { get; set; } = null!;
}
