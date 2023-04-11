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
    public Guid? ParentId { get; set; }
    public Guid UserId { get; set; }
    
}
