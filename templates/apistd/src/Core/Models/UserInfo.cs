namespace Core.Models;

/// <summary>
/// 账号扩展表, 可用作用户信息
/// </summary>
public partial class UserInfo : EntityBase
{
    public User Account { get; set; } = null!;
    public Guid AccountId { get; set; }
    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(40)]
    public string? RealName { get; set; }
    /// <summary>
    /// 昵称 
    /// </summary>
    [MaxLength(40)]
    public string? NickName { get; set; }
    /// <summary>
    /// 证件类型
    /// </summary>
    [MaxLength(20)]
    public string? IdentityType { get; set; }
    /// <summary>
    /// 证件号
    /// </summary>
    [MaxLength(40)]
    public string? IdentityCode { get; set; }
    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTimeOffset? Birthday { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    [MaxLength(100)]
    public string? Address { get; set; }
    /// <summary>
    /// 国家
    /// </summary>
    [MaxLength(20)]
    public string? Country { get; set; }
    /// <summary>
    /// 省
    /// </summary>
    [MaxLength(20)]
    public string? Province { get; set; }
    /// <summary>
    /// 市
    /// </summary>
    [MaxLength(20)]
    public string? City { get; set; }
    /// <summary>
    /// 区县
    /// </summary>
    [MaxLength(20)]
    public string? County { get; set; }
    /// <summary>
    /// 街道
    /// </summary>
    [MaxLength(20)]
    public string? Street { get; set; }
    /// <summary>
    /// 详情地址:路/小区/楼
    /// </summary>
    [MaxLength(50)]
    public string? AddressDetail { get; set; }
    /// <summary>
    /// 微信openId
    /// </summary>
    [MaxLength(40)]
    public string? WXOpenId { get; set; }
    [MaxLength(150)]
    public string? WXAvatar { get; set; }
    [MaxLength(40)]
    public string? WXUnionId { get; set; }

}
