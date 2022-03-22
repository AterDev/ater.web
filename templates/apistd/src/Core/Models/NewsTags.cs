namespace Core.Models;

/// <summary>
/// 新闻标签
/// </summary>
public class NewsTags : EntityBase
{
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Color { get; set; }
    public ThirdNews ThirdNews { get; set; } = null!;

}
