using Core.Entities.ContentEntities;
namespace Share.Models.TagsDtos;
/// <summary>
/// 标签概要
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Tags"/>
public class TagsShortDto
{
    /// <summary>
    /// 标签名称
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 标签颜色
    /// </summary>
    [MaxLength(20)]
    public string? Color { get; set; }
    public User User { get; set; } = default!;
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
