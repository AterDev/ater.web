using System.Text.Json;
using Ater.Web.Abstraction;
using Entity.System;
using EntityFramework.DBProvider;

namespace Http.API.Worker;
public class InitDataTask
{
    public static async Task InitDataAsync(IServiceProvider provider)
    {
        CommandDbContext context = provider.GetRequiredService<CommandDbContext>();
        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        ILogger<InitDataTask> logger = loggerFactory.CreateLogger<InitDataTask>();
        IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

        var connectionString = context.Database.GetConnectionString();
        try
        {
            // 执行迁移,如果手动执行,请删除该代码
            context.Database.Migrate();
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogError("数据库无法连接:{message}", connectionString);
                return;
            }
            else
            {
                // 初始化用户
                var user = await context.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    await InitUserAsync(context, configuration, logger);
                }

                await InitSystemConfigAsync(context, configuration, logger);
            }
        }
        catch (Exception)
        {
            logger.LogError("初始化异常,请检查数据库配置：{message}", connectionString);
        }
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    public static async Task InitUserAsync(CommandDbContext context, IConfiguration configuration, ILogger<InitDataTask> logger)
    {
        var defaultPassword = configuration.GetValue<string>("Key:DefaultPassword");
        if (string.IsNullOrWhiteSpace(defaultPassword))
        {
            defaultPassword = "Hello.Net";
        }
        var salt = HashCrypto.BuildSalt();

        User user = new()
        {
            UserName = "TestUser",
            Email = "TestEmail@domain",
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd(defaultPassword, salt),
        };

        try
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            logger.LogInformation("初始化用户数据成功:{username}/{password}", user.UserName, defaultPassword);

        }
        catch (Exception ex)
        {
            logger.LogError("初始化角色用户时出错,请确认您的数据库没有数据！{message}", ex.Message);
        }
    }

    public static async Task InitSystemConfigAsync(CommandDbContext context, IConfiguration configuration, ILogger<InitDataTask> logger)
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
