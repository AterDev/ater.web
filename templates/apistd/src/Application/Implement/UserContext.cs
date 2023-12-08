using System.Runtime.CompilerServices;
using EntityFramework.DBProvider;
using Microsoft.AspNetCore.Http;

namespace Application.Implement;

public partial class UserContext : IUserContext
{
    public Guid UserId { get; init; }
    public string? Username { get; init; }
    public string? Email { get; set; }
    /// <summary>
    /// �Ƿ�Ϊ����Ա
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
    /// �жϵ�ǰ��ɫ
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public bool IsRole(string roleName)
    {
        return Roles != null && Roles.Any(r => r == roleName);
    }

    /// <summary>
    /// �Ƿ����
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ExistAsync()
    {
        return IsAdmin ?
            await _context.SystemUsers.AnyAsync(u => u.Id == UserId) :
            await _context.Users.AnyAsync(u => u.Id == UserId);
    }

    public async Task<User?> GetUserAsync()
    {
        return await _context.Users.FindAsync(UserId);
    }

    public async Task<SystemUser?> GetSystemUserAsync()
    {
        return await _context.SystemUsers.FindAsync(UserId);
    }

    /// <summary>
    /// ��Ϊ��¼
    /// </summary>
    /// <param name="targetName"></param>
    /// <param name="actionType"></param>
    /// <param name="description"></param>
    /// <param name="caller"></param>
    public async void RecordAction(string targetName, ActionType actionType, string description = "", [CallerMemberName] string caller = "")
    {
        var route = _httpContextAccessor.HttpContext?.Request?.Path.Value + $":[{caller}]";

        if (UserId != Guid.Empty)
        {
            description = "";
            var log = new SystemLogs
            {
                ActionUserName = Username ?? Email ?? UserId.ToString() ?? "",
                TargetName = targetName,
                Route = route,
                ActionType = actionType,
            };
            // get enum description from actionType
            var actionName = actionType.GetDescription();
            log.Description = $"{log.ActionUserName} {actionName} {targetName} ;{description}";
            _context.Entry(log).Property("SystemUserId").CurrentValue = UserId;
            _context.Add(log);
            await _context.SaveChangesAsync();
        }
    }

}
