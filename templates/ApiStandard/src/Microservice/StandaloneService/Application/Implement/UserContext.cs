
namespace StandaloneService.Application.Implement;

public partial class UserContext : IUserContext
{
    public Guid UserId { get; init; }
    public string? Username { get; init; }
    public string? Email { get; set; }
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; init; }
    public string? CurrentRole { get; set; }
    public List<string>? Roles { get; set; }
    public Guid? GroupId { get; init; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CommandDbContext _context;
    public UserContext(IHttpContextAccessor httpContextAccessor, CommandDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        if (Guid.TryParse(FindClaim(ClaimTypes.NameIdentifier)?.Value, out Guid userId) && userId != Guid.Empty)
        {
            UserId = userId;
        }
        if (Guid.TryParse(FindClaim(ClaimTypes.GroupSid)?.Value, out Guid groupSid) && groupSid != Guid.Empty)
        {
            GroupId = groupSid;
        }
        Username = FindClaim(ClaimTypes.Name)?.Value;
        Email = FindClaim(ClaimTypes.Email)?.Value;

        CurrentRole = FindClaim(ClaimTypes.Role)?.Value;

        Roles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
            .Select(c => c.Value).ToList();
        if (Roles != null)
        {
            IsAdmin = Roles.Any(r => r.Equals(AppConst.AdminUser));
        }
        _context = context;
    }

    public Claim? FindClaim(string claimType)
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst(claimType);
    }

    /// <summary>
    /// 判断当前角色
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public bool IsRole(string roleName)
    {
        return Roles != null && Roles.Any(r => r == roleName);
    }

    public async Task<bool> ExistAsync()
    {
        return await Task.FromResult(false);
    }

    /// <summary>
    /// 获取ip地址
    /// </summary>
    /// <returns></returns>
    public string? GetIpAddress()
    {
        HttpRequest? request = _httpContextAccessor.HttpContext?.Request;
        return request == null
            ? string.Empty
            : request.Headers.ContainsKey("X-Forwarded-For")
            ? request.Headers["X-Forwarded-For"].Where(x => x != null).FirstOrDefault()
            : _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString();
    }

    public async Task<TUser?> GetUserAsync<TUser>() where TUser : class
    {
        return await _context.Set<TUser>().FindAsync(UserId);
    }
}
