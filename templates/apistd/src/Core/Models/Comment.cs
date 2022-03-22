namespace Core.Models;

public class Comment : EntityBase
{
    public Article Article { get; set; } = null!;
    public User Account { get; set; } = null!;
    /// <summary>
    /// 评论内容
    /// </summary>
    [MaxLength(2000)]
    public string? Content { get; set; }
}
