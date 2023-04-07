using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Content;
/// <summary>
/// 标签
/// </summary>
[Index(nameof(Name))]
[Index(nameof(Color))]
public class Tags : EntityBase
{
    /// <summary>
    /// 标签名称
    /// </summary>
    [MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// 标签颜色
    /// </summary>
    [MaxLength(20)]
    public string? Color { get; set; }
    public required User User { get; set; }
    public List<Blog>? Blogs { get; set; }
}
