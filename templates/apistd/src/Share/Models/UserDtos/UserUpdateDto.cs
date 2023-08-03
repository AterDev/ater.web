using Entity;
namespace Share.Models.UserDtos;
/// <summary>
/// 用户账户更新时请求结构
/// </summary>
/// <inheritdoc cref="Entity.User"/>
public class UserUpdateDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public string UserName { get; set; } = default!;
    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool? LockoutEnabled { get; set; }
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    
}
