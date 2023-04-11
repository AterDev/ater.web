using Core.Entities;
namespace Share.Models.UserDtos;
/// <summary>
/// 用户账户查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.User"/>
public class UserFilterDto : FilterBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public string? UserName { get; set; }
    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType? UserType { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    // [MaxLength(100)]
    // public string? PasswordHash { get; set; }
    // [MaxLength(60)]
    // public string? PasswordSalt { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool? LockoutEnabled { get; set; }
    public int? AccessFailedCount { get; set; }
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTimeOffset? LastLoginTime { get; set; }
    /// <summary>
    /// 密码重试次数
    /// </summary>
    public int? RetryCount { get; set; }
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    
}
