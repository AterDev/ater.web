using Core.Entities;
namespace Share.Models.UserDtos;
/// <summary>
/// 用户账户查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.User"/>
public class UserFilterDto : FilterBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public string? UserName { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    // [MaxLength(100)]
    // public string? PasswordHash { get; set; }
    // [MaxLength(60)]
    // public string? PasswordSalt { get; set; }
    
}
