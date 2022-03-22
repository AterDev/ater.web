namespace Core.Models;

/// <summary>
/// 文章内容
/// </summary>
public partial class Article : EntityBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    /// <summary>
    /// 概要
    /// </summary>
    [MaxLength(500)]
    public string? Summary { get; set; }
    /// <summary>
    /// 作者名称
    /// </summary>
    [MaxLength(100)]
    public string? AuthorName { get; set; }
    /// <summary>
    /// 标签
    /// </summary>
    [MaxLength(100)]
    public string? Tags { get; set; }
    /// <summary>
    /// 文章类别
    /// </summary>
    public ArticleType ArticleType { get; set; }
    public User Account { get; set; } = null!;
    /// <summary>
    /// 仅个人查看
    /// </summary>
    public bool? IsPrivate { get; set; }
    /// <summary>
    /// 文章扩展内容
    /// </summary>
    public ArticleExtend? Extend { get; set; }
    /// <summary>
    /// 所属目录
    /// </summary>
    public ArticleCatalog? Catalog { get; set; }
    /// <summary>
    /// 评论
    /// </summary>
    public List<Comment>? Comments { get; set; }
}

public enum ArticleType
{
    /// <summary>
    /// 翻译转载
    /// </summary>
    Transport,
    /// <summary>
    /// 教程
    /// </summary>
    Course,
    /// <summary>
    /// 新闻 
    /// </summary>
    News,
    /// <summary>
    /// 技术分享
    /// </summary>
    Tech,
    /// <summary>
    /// 评论
    /// </summary>
    Comment
}
