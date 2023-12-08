namespace OrderMod.Models.ProductDtos;
/// <summary>
/// 产品概要
/// </summary>
/// <see cref="Entity.Order.Product"/>
public class ProductShortDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(60)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// 原价
    /// </summary>
    public decimal OriginPrice { get; set; }
    public Guid Id { get; set; }
    
}
