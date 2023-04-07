using Core.Entities.System;

namespace Share.Models.SystemUserDtos;
/// <summary>
/// 系统用户概要
/// </summary>
public class SystemUserShortDto
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
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; } = default!;
    // [MaxLength(100)]
    // public string PasswordHash { get; set; }
    // [MaxLength(60)]
    // public string PasswordSalt { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; } = default!;
    public bool TwoFactorEnabled { get; set; } = default!;
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; } = default!;
    public int AccessFailedCount { get; set; } = default!;
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTimeOffset? LastLoginTime { get; set; }
    /// <summary>
    /// 密码重试次数
    /// </summary>
    public int RetryCount { get; set; } = default!;
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }


    /// <summary>
    /// 性别
    /// </summary>
    public Sex Sex { get; set; } = default!;
    public Guid Id { get; set; } = default!;
    public DateTimeOffset CreatedTime { get; set; } = default!;
    public DateTimeOffset UpdatedTime { get; set; } = default!;
    /// <summary>
    /// 软删除
    /// </summary>
    public bool IsDeleted { get; set; } = default!;

}
