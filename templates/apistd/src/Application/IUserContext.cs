namespace Application;
public interface IUserContext : IUserContextBase
{
    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <returns></returns>
    Task<bool> ExistAsync();
    Claim? FindClaim(string claimType);
    Task<User?> GetUserAsync();
}
