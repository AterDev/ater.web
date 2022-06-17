namespace Share.Models.AuthDtos;

public class AuthResult
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Token { get; set; } = default!;
}
