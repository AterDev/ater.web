using Core.Entities;
namespace Share.Models.SystemUserDtos;
/// <summary>
/// 系统用户查询筛选
/// </summary>
public class SystemUserFilterDto : FilterBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(30)]
    public string? UserName { get; set; }
    public bool? EmailConfirmed { get; set; }
    // [MaxLength(100)]
    // public string? PasswordHash { get; set; }
    // [MaxLength(60)]
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
    public Sex? Sex { get; set; }
    
}
