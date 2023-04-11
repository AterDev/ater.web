using Core.Entities;
namespace Share.Models.UserDtos;
/// <summary>
/// 用户账户添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.User"/>
public class UserAddDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public required string UserName { get; set; }
    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; } = UserType.Normal;
    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    [MaxLength(100)]
    public string PasswordHash { get; set; } = default!;
    [MaxLength(60)]
    public string PasswordSalt { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; } = false;
    public int AccessFailedCount { get; set; } = 0;
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTimeOffset? LastLoginTime { get; set; }
    /// <summary>
    /// 密码重试次数
    /// </summary>
    public int RetryCount { get; set; } = 0;
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    
}
