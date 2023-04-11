using Core.Entities.ContentEntities;
namespace Share.Models.CatalogDtos;
/// <summary>
/// 目录添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Catalog"/>
public class CatalogAddDto
{
    /// <summary>
    /// 目录名称
    /// </summary>
    [MaxLength(50)]
    public required string Name { get; set; }
    /// <summary>
    /// 层级
    /// </summary>
    public short Level { get; set; } = 0;
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
    public required User User { get; set; }
    public Guid UserId { get; set; }
    
}
