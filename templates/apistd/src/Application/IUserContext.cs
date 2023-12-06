namespace Application;

/// <summary>
/// 用户上下文
/// </summary>
public interface IUserContext
{
    public Guid UserId { get; init; }
    public string? Username { get; init; }
    public string? Email { get; set; }
    public bool IsAdmin { get; init; }
    public string? CurrentRole { get; set; }
    public List<string>? Roles { get; set; }
    /// <summary>
    /// 组织id
    /// </summary>
    public Guid? GroupId { get; init; }

    Task<bool> ExistAsync();
    Claim? FindClaim(string claimType);
    Task<SystemUser?> GetSystemUserAsync();
    Task<User?> GetUserAsync();
    bool IsRole(string roleName);
}
