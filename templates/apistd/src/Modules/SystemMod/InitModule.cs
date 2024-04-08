using System.Text.Json;
using EntityFramework.DBProvider;

namespace SystemMod;
public class InitModule
{
    /// <summary>
    /// 模块初始化方法
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static async Task InitializeAsync(IServiceProvider provider)
    {
        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        CommandDbContext context = provider.GetRequiredService<CommandDbContext>();
        ILogger<InitModule> logger = loggerFactory.CreateLogger<InitModule>();
        IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
        CacheService cache = provider.GetRequiredService<CacheService>();

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
                // 初始化管理员账号和角色
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
                // 初始化配置
                await InitConfigAsync(context, configuration, logger);
            }
            await InitCacheAsync(context, cache, logger);
        }
        catch (Exception ex)
        {
            logger.LogError("初始化系统配置失败！{message}", ex.Message);
        }
    }

    /// <summary>
    /// 初始化配置
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private static async Task InitConfigAsync(CommandDbContext context, IConfiguration configuration, ILogger logger)
    {
        // 初始化配置信息
        var initConfig = SystemConfig.NewSystemConfig(AppConst.SystemGroup, AppConst.IsInit, "true");

        var loginSecurityPolicy = configuration.GetSection(AppConst.LoginSecurityPolicy)
            .Get<LoginSecurityPolicy>() ?? new LoginSecurityPolicy();

        var loginSecurityPolicyConfig = SystemConfig.NewSystemConfig(AppConst.SystemGroup, AppConst.LoginSecurityPolicy, JsonSerializer.Serialize(loginSecurityPolicy));

        context.SystemConfigs.Add(loginSecurityPolicyConfig);
        context.SystemConfigs.Add(initConfig);

        await context.SaveChangesAsync();

        logger.LogInformation("写入登录安全策略成功");
    }

    /// <summary>
    /// 加载配置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cache"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    private static async Task InitCacheAsync(CommandDbContext context, CacheService cache, ILogger logger)
    {
        logger.LogInformation("加载配置缓存");
        var securityPolicy = context.SystemConfigs
            .Where(c => c.Key.Equals(AppConst.LoginSecurityPolicy))
            .Where(c => c.GroupName.Equals(AppConst.SystemGroup))
            .Select(c => c.Value)
            .FirstOrDefault();

        if (securityPolicy != null)
        {
            await cache.SetValueAsync(AppConst.LoginSecurityPolicy, securityPolicy, null);
        }
    }
}
