namespace Entity.Order;
/// <summary>
/// 商品
/// </summary>
[Module(Modules.Order)]
[Index(nameof(Sort))]
[Index(nameof(Name))]
[Index(nameof(ProductType))]
public class Product : IEntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(60)]
    public required string Name { get; set; }

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
    /// 积分兑换
    /// </summary>
    public int CostScore { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 有效期：天
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 产品类型
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 原价
    /// </summary>
    public decimal OriginPrice { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}

/// <summary>
/// 产品类型
/// </summary>
public enum ProductType
{
    /// <summary>
    /// 商品
    /// </summary>
    [Description("商品")]
    Goods,
    /// <summary>
    /// 服务
    /// </summary>
    [Description("服务")]
    Service,
    /// <summary>
    /// 会员
    /// </summary>
    [Description("会员")]
    Member,
    /// <summary>
    /// 高级会员
    /// </summary>
    [Description("高级会员")]
    AdvancedMember,
}
