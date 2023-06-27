namespace Share.Models.AuthDtos;

/// <summary>
/// 登录
/// </summary>
public class LoginDto
{
    [MaxLength(50)]
    public string UserName { get; set; } = default!;
    [MaxLength(60)]
    public string Password { get; set; } = default!;

}
