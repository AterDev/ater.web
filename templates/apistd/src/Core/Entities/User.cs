using Microsoft.EntityFrameworkCore;

namespace Core.Entities;
/// <summary>
/// 用户账户
/// </summary>
[Index(nameof(UserName))]
[Index(nameof(Email))]
[Index(nameof(PhoneNumber))]
[Index(nameof(CreatedTime))]
[Index(nameof(IsDeleted))]
public class User : EntityBase
{
    // TODO:根据实际需求调整字段

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
    public string? Email { get; set; } = null!;
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
public enum UserType
{
    /// <summary>
    /// 普通用户
    /// </summary>
    Normal,
    /// <summary>
    /// 认证用户
    /// </summary>
    Verify,
    /// <summary>
    /// 会员
    /// </summary>
    Member
}
