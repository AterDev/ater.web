using Microsoft.AspNetCore.Http;

namespace Application.Implement;

public class UserContext : IUserContext
{
    public Guid? UserId { get; init; }
    public Guid? SessionId { get; init; }
    public string? Username { get; init; }
    public string? Email { get; set; }
    public bool IsAdmin { get; init; } = false;
    public string? CurrentRole { get; set; }
    public List<string>? Roles { get; set; }
    public Guid? GroupId { get; init; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CommandDbContext _context;
    public UserContext(IHttpContextAccessor httpContextAccessor, CommandDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        if (Guid.TryParse(FindClaim(ClaimTypes.NameIdentifier)?.Value, out var userId) && userId != Guid.Empty)
        {
            UserId = userId;
        }
        if (Guid.TryParse(FindClaim(ClaimTypes.GroupSid)?.Value, out var groupSid) && groupSid != Guid.Empty)
        {
            GroupId = groupSid;
        }
        Username = FindClaim(ClaimTypes.Name)?.Value;
        Email = FindClaim(ClaimTypes.Email)?.Value;
        CurrentRole = FindClaim(ClaimTypes.Role)?.Value;
        if (CurrentRole != null && CurrentRole.ToLower() == "admin")
        {
            IsAdmin = true;
        }
        else
        {
            IsAdmin = false;
        }
        _context = context;
    }

    public Claim? FindClaim(string claimType) => _httpContextAccessor?.HttpContext?.User?.FindFirst(claimType);
    public async Task<User?> GetUserAsync()
    {
        return await _context.Users.FindAsync(UserId);
    }
}
