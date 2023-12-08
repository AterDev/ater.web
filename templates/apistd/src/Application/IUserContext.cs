namespace Application;
public interface IUserContext : IUserContextBase
{
    Task<bool> ExistAsync();
    Claim? FindClaim(string claimType);
    Task<SystemUser?> GetSystemUserAsync();
    Task<User?> GetUserAsync();
}
