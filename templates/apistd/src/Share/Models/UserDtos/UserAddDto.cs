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
    [MaxLength(60)]
    public string UserName { get; set; } = default!;
    /// <summary>
    /// 密码
    /// </summary>
    [MaxLength(100)]
    public string Password { get; set; } = default!;
    [MaxLength(100)]
    public string? Email { get; set; }
    
}
