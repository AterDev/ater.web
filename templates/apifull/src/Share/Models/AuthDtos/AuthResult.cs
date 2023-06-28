namespace Share.Models.AuthDtos;

public class AuthResult
{
    public Guid Id { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = default!;
    public string[] Roles { get; set; } = default!;
    /// <summary>
    /// token
    /// </summary>
    public string Token { get; set; } = default!;
}
