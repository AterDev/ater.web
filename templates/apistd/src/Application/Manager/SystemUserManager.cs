using Application.Services;
using Microsoft.Extensions.Configuration;
using Share.Models.SystemUserDtos;
using Share.Options;

namespace Application.Manager;

public class SystemUserManager : DomainManagerBase<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>, ISystemUserManager
{
    private readonly CacheService _cache;
    private readonly IConfiguration _config;

    private string? ErrorMessage { get; }

    public SystemUserManager(
        DataStoreContext storeContext,
        ILogger<SystemUserManager> logger,
        IUserContext userContext,
        CacheService cache,
        IConfiguration config) : base(storeContext, logger)
    {
        _userContext = userContext;
        _cache = cache;
        _config = config;
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
    /// 发送验证码
    /// </summary>
    /// <param name="email"></param>
    /// <param name="subject"></param>
    /// <param name="htmlContent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task SendVerifyEmailAsync(string email, string subject, string htmlContent)
    {
        var smtp = _config.GetSection("Smtp").Get<SmtpOption>() ?? throw new ArgumentNullException("未找到Smtp选项!");

        await SmtpService.Create(smtp.Host, smtp.Port, true).SetCredentials(smtp.Username, smtp.Password)
            .SendEmailAsync(smtp.DisplayName, smtp.From, email, subject, htmlContent);
    }

    /// <summary>
    /// 获取用户角色权限信息
    /// </summary>
    /// <returns></returns>
    public void LoadRolesWithPermissions(SystemUser user)
    {
        Stores.QueryContext.Entry(user)
            .Collection(u => u.SystemRoles)
            .Query()
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
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemUser> UpdateAsync(SystemUser entity, SystemUserUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemUserItemDto>> FilterAsync(SystemUserFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.UserName, q => q.UserName == filter.UserName);
        // TODO: custom filter conditions
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
