using Core.Entities;
namespace Share.Models.UserDtos;
/// <summary>
/// 用户账户列表元素
/// </summary>
/// <inheritdoc cref="Core.Entities.User"/>
public class UserItemDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public string UserName { get; set; } = default!;
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    // [MaxLength(100)]
    // public string PasswordHash { get; set; } = default!;
    // [MaxLength(60)]
    // public string PasswordSalt { get; set; } = default!;
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
