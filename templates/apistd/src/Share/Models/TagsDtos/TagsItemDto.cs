using Core.Entities.ContentEntities;
namespace Share.Models.TagsDtos;
/// <summary>
/// 标签列表元素
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Tags"/>
public class TagsItemDto
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
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
