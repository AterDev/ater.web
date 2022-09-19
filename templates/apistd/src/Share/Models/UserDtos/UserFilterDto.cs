namespace Share.Models.UserDtos;
/// <summary>
/// 系统用户查询筛选
/// </summary>
public class UserFilterDto : FilterBase
{
    public string? UserName { get; set; }
    public bool? EmailConfirmed { get; set; }
    // public string? PasswordHash { get; set; }
    // public string? PasswordSalt { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public bool? LockoutEnabled { get; set; }
    public int? AccessFailedCount { get; set; }
    /// <summary>
    /// 密码重试次数
    /// </summary>
    public int? RetryCount { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public SexType? Sex { get; set; }
}
