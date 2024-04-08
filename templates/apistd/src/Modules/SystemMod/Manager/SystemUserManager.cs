using System.Text.RegularExpressions;
using Ater.Web.Extension;
using Share.Models.UserDtos;
using SystemMod.Models;
using SystemMod.Models.SystemUserDtos;
using SystemMod.Worker;

namespace SystemMod.Manager;

public class SystemUserManager(
    DataAccessContext<SystemUser> dataContext,
    IUserContext userContext,
    IConfiguration configuration,
    CacheService cache,
    SystemConfigManager systemConfig,
    SystemLogTaskQueue taskQueue,
    ILogger<SystemUserManager> logger) : ManagerBase<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>(dataContext, logger)
{
    private readonly IUserContext _userContext = userContext;
    private readonly SystemConfigManager _systemConfig = systemConfig;
    private readonly SystemLogTaskQueue _taskQueue = taskQueue;
    private readonly IConfiguration _configuration = configuration;
    private readonly CacheService _cache = cache;

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
    /// 获取图形验证码
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public byte[] GetCaptchaImage(int length = 4)
    {
        var code = GetCaptcha(length);
        var width = length * 20;
        return ImageHelper.GenerateImageCaptcha(code, width);
    }

    /// <summary>
    /// 登录安全策略验证
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="user"></param>
    /// <param name="loginPolicy"></param>
    /// <returns></returns>
    public async Task<bool> ValidateLoginAsync(LoginDto dto, SystemUser user, LoginSecurityPolicy loginPolicy)
    {
        if (loginPolicy == null)
        {
            return true;
        }
        // 刷新锁定状态
        var lastLoginTime = user.LastLoginTime?.ToLocalTime() ?? DateTimeOffset.Now;
        if ((DateTimeOffset.Now - lastLoginTime).Days >= 1)
        {
            user.RetryCount = 0;
            if (user.LockoutEnabled)
            {
                user.LockoutEnabled = false;
            }
        }
        // 登录记录
        user.RetryCount++;
        user.LastLoginTime = DateTimeOffset.UtcNow;

        // 锁定状态
        if (user.LockoutEnabled || user.RetryCount >= loginPolicy.LoginRetry)
        {
            user.LockoutEnabled = true;
            ErrorStatus = 500001;
            return false;
        }

        // 验证码处理
        if (loginPolicy.IsNeedVerifyCode)
        {
            if (dto.VerifyCode == null)
            {
                user.RetryCount++;
                ErrorStatus = 500002;
                return false;
            }
            var key = AppConst.VerifyCodeCachePrefix + user.Email;
            string? code = _cache.GetValue<string>(key);
            if (code == null)
            {
                ErrorStatus = 500003;
                user.RetryCount++;
                return false;
            }
            if (!code.Equals(dto.VerifyCode))
            {
                await _cache.RemoveAsync(key);
                ErrorStatus = 500002;
                user.RetryCount++;
                return false;
            }
        }

        // 密码过期时间
        if ((DateTimeOffset.UtcNow - user.LastPwdEditTime).TotalDays > loginPolicy.PasswordExpired * 30)
        {
            ErrorStatus = 500004;
            user.RetryCount++;
            return false;
        }

        if (!HashCrypto.Validate(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            ErrorStatus = 500005;
            return false;
        }
        return true;
    }

    /// <summary>
    /// 生成jwtToken
    /// </summary>
    /// <param name="user"></param>
    /// <param name="jwtOption"></param>
    /// <returns></returns>
    public AuthResult GenerateJwtToken(SystemUser user, JwtOption jwtOption)
    {
        if (jwtOption.Sign.NotEmpty()
            && jwtOption.ValidIssuer.NotEmpty()
            && jwtOption.ValidAudiences.NotEmpty())
        {
            // 加载关联数据
            List<string> roles = user.SystemRoles?.Select(r => r.NameValue)?.ToList()
                ?? [AppConst.AdminUser];
            // 过期时间:秒
            var expiredSeconds = jwtOption.ExpiredSeconds * 60 * 60;

            JwtService jwt = new(jwtOption.Sign, jwtOption.ValidAudiences, jwtOption.ValidIssuer)
            {
                TokenExpires = expiredSeconds,
            };
            // 添加管理员用户标识
            if (!roles.Contains(AppConst.AdminUser))
            {
                roles.Add(AppConst.AdminUser);
            }
            var token = jwt.GetToken(user.Id.ToString(), [.. roles]);

            return new AuthResult
            {
                Id = user.Id,
                Roles = [.. roles],
                Token = token,
                Username = user.UserName
            };
        }
        else
        {
            throw new ArgumentNullException("缺少JWT配置，请联系管理员");
        }
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
        return await Command.SaveChangesAsync() > 0;
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
        if (dto.RoleIds != null && dto.RoleIds.Count != 0)
        {
            IQueryable<SystemRole> roles = CommandContext.SystemRoles.Where(r => dto.RoleIds.Contains(r.Id));
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
        query = query.Where(q => q.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// 验证密码复杂度
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool ValidatePassword(string password)
    {
        var loginPolicy = _systemConfig.GetLoginSecurityPolicy();
        // 密码复杂度校验
        string pwdReg = loginPolicy.PasswordLevel switch
        {
            PasswordLevel.Simple => "",
            PasswordLevel.Normal => "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,60}$",
            PasswordLevel.Strict => "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$",
            _ => "^.{6,16}$"
        };
        if (!Regex.IsMatch(password, pwdReg))
        {
            ErrorMsg = loginPolicy.PasswordLevel switch
            {
                PasswordLevel.Simple => "密码长度6-60位",
                PasswordLevel.Normal => "密码长度8-60位，必须包含大小写字母和数字",
                PasswordLevel.Strict => "密码长度8位以上，必须包含大小写字母、数字和特殊字符",
                _ => "密码长度6-16位"
            };
            return false;
        }
        return true;
    }

    /// <summary>
    /// 保存登录记录
    /// </summary>
    /// <param name="user"></param>
    /// <param name="description"></param>
    public async Task SaveLoginLogAsync(SystemUser user, string description)
    {
        var log = SystemLogs.NewLog(user.UserName, user.Id, "登录", ActionType.Login, description: description);
        await _taskQueue.AddLogItemAsync(log);
    }
}
