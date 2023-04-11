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
    public Guid? ParentId { get; set; }
    public Guid UserId { get; set; } = default!;
    
}
