namespace Share.Models.UserDtos;

/// <summary>
/// 用户注册
/// </summary>
/// <inheritdoc cref="Entity.User"/>
public class RegisterDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(40)]
    public required string UserName { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string? VerifyCode { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    [MaxLength(60)]
    public required string Password { get; set; }
}
