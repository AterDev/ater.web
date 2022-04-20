namespace Core.Models;
/// <summary>
/// 系统用户
/// </summary>
public class User : EntityBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(30)]
    public string UserName { get; set; } = default!;
    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(30)]
    public string? RealName { get; set; }
    /// <summary>
    /// 职位
    /// </summary>
    [MaxLength(30)]
    public string? Position { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; } = false;
    [MaxLength(100)]
    public string PasswordHash { get; set; } = default!;
    [MaxLength(60)]
    public string PasswordSalt { get; set; } = default!;
    [MaxLength(20)]
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
    public bool IsDeleted { get; set; } = false;
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    public List<Role>? Roles { get; set; }
}

