using System.Security.Claims;
using Entity;
using Entity.SystemEntities;

namespace Application.Test;

public class TestUserContext : IUserContext
{
    public Guid? UserId { get; init; } = new Guid("6e2bb78f-fa51-480d-8200-83d488184621");
    public Guid? SessionId { get; init; }
    public string? Username { get; init; } = "TestUser";
    public string? Email { get; set; } = "TestEmail@dusi.dev";
    public bool IsAdmin { get; init; } = false;
    public string? CurrentRole { get; set; } = "Test";
    public List<string>? Roles { get; set; }
    public Guid? GroupId { get; init; }

    public TestUserContext()
    {
    }

    public Task<bool> ExistAsync()
    {
        return Task.FromResult(true);
    }

    public Claim? FindClaim(string claimType)
    {
        throw new NotImplementedException();
    }

    public Task<SystemUser?> GetSystemUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserAsync()
    {
        var user = new User()
        {
            Id = UserId!.Value,
            UserName = Username!,
            Email = Email,
        };
        return Task.FromResult(user)!;
    }

    public bool IsRole(string roleName)
    {
        return true;
    }
}