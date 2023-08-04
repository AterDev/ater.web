using Share.Models.SystemUserDtos;

namespace Application.Manager;

public class SystemUserManager : DomainManagerBase<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>, ISystemUserManager
{

    private string? ErrorMessage { get; }

    public SystemUserManager(
        DataStoreContext storeContext,
        ILogger<SystemUserManager> logger,
        IUserContext userContext) : base(storeContext, logger)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// 获取验证码
    /// 也可自己实现图片验证码
    /// </summary>
    /// <param name="length">验证码长度</param>
    /// <returns></returns>
    public string GetCaptcha(int length = 6)
    {
        return HashCrypto.GetRnd(length);
    }

    /// <summary>
    /// 更新密码
    /// </summary>
    /// <param name="user"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    public async Task<bool> ChangePasswordAsync(SystemUser user, string newPassword)
    {
        user.PasswordSalt = HashCrypto.BuildSalt();
        user.PasswordHash = HashCrypto.GeneratePwd(newPassword, user.PasswordSalt);
        Command.Update(user);
        return await Command.SaveChangeAsync() > 0;
    }

    /// <summary>
    /// 获取用户角色权限信息
    /// </summary>
    /// <returns></returns>
    public void LoadRolesWithPermissions(SystemUser user)
    {
        Stores.CommandContext.Entry(user)
            .Collection(u => u.SystemRoles)
            .Query()
            .AsNoTracking()
            .Include(r => r.Menus)
            .Include(r => r.PermissionGroups)
                .ThenInclude(g => g.Permissions)
            .Load();
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto)
    {
        SystemUser entity = dto.MapTo<SystemUserAddDto, SystemUser>();
        // 密码处理
        entity.PasswordSalt = HashCrypto.BuildSalt();
        entity.PasswordHash = HashCrypto.GeneratePwd(dto.Password, entity.PasswordSalt);
        // 角色处理
        if (dto.RoleIds != null && dto.RoleIds.Any())
        {
            var roles = Stores.CommandContext.SystemRoles.Where(r => dto.RoleIds.Contains(r.Id));
            entity.SystemRoles = roles.ToList();
        }
        return Task.FromResult(entity);
    }

    public override async Task<SystemUser> UpdateAsync(SystemUser entity, SystemUserUpdateDto dto)
    {

        if (dto.Password != null)
        {
            entity.PasswordSalt = HashCrypto.BuildSalt();
            entity.PasswordHash = HashCrypto.GeneratePwd(dto.Password, entity.PasswordSalt);
        }

        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemUserItemDto>> FilterAsync(SystemUserFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.UserName, q => q.UserName == filter.UserName)
            .WhereNotNull(filter.RealName, q => q.RealName == filter.RealName)
            .WhereNotNull(filter.Email, q => q.Email == filter.Email)
            .WhereNotNull(filter.PhoneNumber, q => q.PhoneNumber == filter.PhoneNumber)
            .WhereNotNull(filter.Sex, q => q.Sex == filter.Sex)
            .WhereNotNull(filter.EmailConfirmed, q => q.EmailConfirmed == filter.EmailConfirmed)
            .WhereNotNull(filter.PhoneNumberConfirmed, q => q.PhoneNumberConfirmed == filter.PhoneNumberConfirmed);

        if (filter.RoleId != null)
        {
            Queryable = Queryable.Where(q => q.SystemRoles.Any(r => r.Id == filter.RoleId));
        }

        return await Query.FilterAsync<SystemUserItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemUser?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemUser> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
