using Core.Entities.ContentEntities;
namespace Share.Models.TagsDtos;
/// <summary>
/// 标签添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Tags"/>
public class TagsAddDto
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
    public Guid UserId { get; set; }
    
}
