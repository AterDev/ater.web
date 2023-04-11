using Core.Entities.ContentEntities;
namespace Share.Models.CatalogDtos;
/// <summary>
/// 目录更新时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Catalog"/>
public class CatalogUpdateDto
{
    /// <summary>
    /// 目录名称
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 层级
    /// </summary>
    public short? Level { get; set; }
    /// <summary>
    /// 子目录
    /// </summary>
    public List<Catalog>? Children { get; set; }
    /// <summary>
    /// 父目录
    /// </summary>
    public Catalog? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public List<Blog>? Blogs { get; set; }
    public User User { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
    
}
