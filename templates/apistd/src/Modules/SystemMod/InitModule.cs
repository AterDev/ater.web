using System.Text.Json;
using EntityFramework.DBProvider;
using Share.Options;

namespace SystemMod;
public static class InitModule
{
    /// <summary>
    /// 模块初始化方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static async Task InitializeAsync(CommandDbContext context, IConfiguration configuration, ILogger logger)
    {
        var isInitString = context.SystemConfigs.Where(c => c.Key.Equals(AppConst.IsInit))
            .Where(c => c.GroupName.Equals(AppConst.SystemGroup))
            .Select(c => c.Value)
            .FirstOrDefault();
        try
        {
            // 未初始化时
            if (isInitString.IsEmpty() || isInitString.Equals("false"))
            {
                logger.LogInformation("⛏️ 开始初始化系统");
                SystemRole? role = await context.SystemRoles.SingleOrDefaultAsync(r => r.NameValue == AppConst.AdminUser);

                if (role == null)
                {
                    var defaultPassword = configuration.GetValue<string>("Key:DefaultPassword");
                    if (string.IsNullOrWhiteSpace(defaultPassword))
                    {
                        defaultPassword = "Hello.Net";
                    }
                    SystemRole superRole = new()
                    {
                        Name = AppConst.SuperAdmin,
                        NameValue = AppConst.SuperAdmin,
                    };
                    SystemRole adminRole = new()
                    {
                        Name = AppConst.AdminUser,
                        NameValue = AppConst.AdminUser,
                    };
                    var salt = HashCrypto.BuildSalt();
                    SystemUser systemUser = new()
                    {
                        UserName = "admin",
                        PasswordSalt = salt,
                        PasswordHash = HashCrypto.GeneratePwd(defaultPassword, salt),
                        SystemRoles = [superRole, adminRole],
                    };

                    context.SystemUsers.Add(systemUser);
                    await context.SaveChangesAsync();
                    logger.LogInformation("初始化管理员账号:{username}/{password}", systemUser.UserName, defaultPassword);
                }

                var initConfig = SystemConfig.NewSystemConfig(AppConst.SystemGroup, AppConst.IsInit, "true");
                var loginSecurityPolicy = new LoginSecurityPolicy();
                var loginSecurityPolicyConfig = SystemConfig.NewSystemConfig(AppConst.SystemGroup, AppConst.LoginSecurityPolicy, JsonSerializer.Serialize(loginSecurityPolicy));

                context.SystemConfigs.Add(initConfig);
                context.SystemConfigs.Add(loginSecurityPolicyConfig);
                await context.SaveChangesAsync();

                logger.LogInformation("写入登录安全策略成功");
            }
        }
        catch (Exception ex)
        {
            logger.LogError("初始化系统配置失败！{message}", ex.Message);
        }
    }

}
