using Core.Entities.ContentEntities;
namespace Share.Models.BlogDtos;
/// <summary>
/// 博客概要
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Blog"/>
public class BlogShortDto
{
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(200)]
    public string? TranslateTitle { get; set; }
    /// <summary>
    /// 语言类型
    /// </summary>
    public LanguageType LanguageType { get; set; } = LanguageType.CN;
    /// <summary>
    /// 全站类别
    /// </summary>
    public BlogType BlogType { get; set; }
    /// <summary>
    /// 是否审核
    /// </summary>
    public bool IsAudit { get; set; } = false;
    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublic { get; set; } = true;
    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }
    public User User { get; set; } = default!;
    /// <summary>
    /// 所属目录
    /// </summary>
    public Catalog Catalog { get; set; } = default!;
    /// <summary>
    /// 浏览量
    /// </summary>
    public int ViewCount { get; set; } = 0;
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(100)]
    public string Title { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(300)]
    public string? Description { get; set; }
    /// <summary>
    /// 作者
    /// </summary>
    [MaxLength(200)]
    public string Authors { get; set; } = default!;
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
