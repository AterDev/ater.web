using Core.Entities.ContentEntities;
namespace Share.Models.CatalogDtos;
/// <summary>
/// 目录查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.ContentEntities.Catalog"/>
public class CatalogFilterDto : FilterBase
{
    /// <summary>
    /// 目录名称
    /// </summary>
    [MaxLength(50)]
    public string? Name { get; set; }
    /// <summary>
    /// 层级
    /// </summary>
    public short? Level { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? UserId { get; set; }
    
}
