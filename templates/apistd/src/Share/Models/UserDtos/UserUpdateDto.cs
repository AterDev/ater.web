namespace Share.Models.UserDtos;
/// <summary>
/// 系统用户更新时请求结构
/// </summary>
public class UserUpdateDto
{
    public string? UserName { get; set; }
    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(30)]
    public string? RealName { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    // public string? PasswordHash { get; set; }
    // public string? PasswordSalt { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength(20)]
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
    /// <summary>
    /// 身份证号
    /// </summary>
    [MaxLength(18)]
    public string? IdNumber { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public SexType? Sex { get; set; }
    
}
