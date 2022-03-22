namespace Core.Models;

/// <summary>
/// 模型库
/// </summary>
public class Library : EntityBase
{
    /// <summary>
    /// 库命名空间
    /// </summary>
    [MaxLength(100)]
    public string Namespace { get; set; } = null!;
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    /// <summary>
    /// 语言类型
    /// </summary>
    [MaxLength(100)]
    public string? Language { get; set; }
    /// <summary>
    /// 是否有效
    /// </summary>
    public bool IsValid { get; set; } = true;
    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublic { get; set; } = false;
    public User User { get; set; } = null!;
    public LibraryCatalog? Catalog { get; set; }
    public List<CodeSnippet>? Snippets { get; set; }
}
