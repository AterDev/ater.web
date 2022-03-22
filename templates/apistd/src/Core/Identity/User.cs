using Microsoft.AspNetCore.Identity;

namespace Core.Identity;
/// <summary>
/// 账号表
/// </summary>
public class User : IdentityUser<Guid>
{
    [PersonalData]
    public override Guid Id { get; set; }
    [ProtectedPersonalData]
    public override string UserName { get; set; } = null!;
    public override string? NormalizedUserName { get; set; }
    [ProtectedPersonalData]
    public override string Email { get; set; } = null!;
    public override string? NormalizedEmail { get; set; }
    [PersonalData]
    public override bool EmailConfirmed { get; set; } = false;
    public override string PasswordHash { get; set; } = null!;
    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public override string? SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [ProtectedPersonalData]
    public override string? PhoneNumber { get; set; }
    [PersonalData]
    public override bool PhoneNumberConfirmed { get; set; } = false;
    [PersonalData]
    public override bool TwoFactorEnabled { get; set; } = false;
    public override DateTimeOffset? LockoutEnd { get; set; }
    public override bool LockoutEnabled { get; set; } = false;
    public override int AccessFailedCount { get; set; } = 0;
    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTimeOffset? LastLoginTime { get; set; }
    /// <summary>
    /// 软删除
    /// </summary>
    public bool IsDeleted { get; set; } = false;
    /// <summary>
    /// 密码重试次数
    /// </summary>
    public int RetryCount { get; set; } = 0;
    public Status Status { get; set; } = Status.Default;
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    public UserInfo? Extend { get; set; }
    /// <summary>
    /// 文章
    /// </summary>
    public List<Article>? Articles { get; set; }
    /// <summary>
    /// 文章目录
    /// </summary>
    public List<ArticleCatalog>? ArticleCatalogs { get; set; }
    /// <summary>
    /// 评论
    /// </summary>
    public List<Comment>? Comments { get; set; }
}

