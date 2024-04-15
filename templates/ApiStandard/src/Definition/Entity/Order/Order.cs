using Ater.Web.Core.Utils;

namespace Entity.Order;
/// <summary>
/// 订单
/// </summary>
[Module(Modules.Order)]
[Index(nameof(DiscountCode))]
[Index(nameof(Status))]
public class Order : IEntityBase
{
    /// <summary>
    /// 订单编号 
    /// </summary>
    [MaxLength(30)]
    public string OrderNumber { get; set; } = HashCrypto.GetRnd() + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

    /// <summary>
    /// 支付订单号
    /// </summary>
    [MaxLength(100)]
    public string? PayNumber { get; set; }

    /// <summary>
    /// 对应产品
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [MaxLength(60)]
    public required string ProductName { get; set; }

    /// <summary>
    /// 客户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }

    /// <summary>
    /// 原价格
    /// </summary>
    public decimal OriginPrice { get; set; }
    /// <summary>
    /// 支付价格
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// 优惠码
    /// </summary>
    [MaxLength(10)]
    public string? DiscountCode { get; set; }

    /// <summary>
    /// 订单当前状态。
    /// </summary>
    public OrderStatus Status { get; set; }
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}
/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// 未支付
    /// </summary>
    [Description("未支付")]
    UnPaid,
    /// <summary>
    /// 已取消
    /// </summary>
    [Description("已取消")]
    Cancelled,
    /// <summary>
    /// 已支付
    /// </summary>
    [Description("已支付")]
    Paid
}
