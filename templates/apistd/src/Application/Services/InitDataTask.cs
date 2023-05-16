using Core.Const;
using Microsoft.Extensions.Configuration;

namespace Application.Services;
public class InitDataTask
{
    public static async Task InitDataAsync(IServiceProvider provider)
    {
        CommandDbContext context = provider.GetRequiredService<CommandDbContext>();
        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        ILogger<InitDataTask> logger = loggerFactory.CreateLogger<InitDataTask>();
        IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

        string? connectionString = context.Database.GetConnectionString();
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
                // 判断是否初始化
                SystemRole? role = await context.SystemRoles.SingleOrDefaultAsync(r => r.NameValue == Const.AdminUser);
                if (role == null)
                {
                    logger.LogInformation("初始化数据");
                    await InitRoleAndUserAsync(context, configuration, logger);
                }
                await UpdateAsync(context, configuration);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("初始化异常,请检查数据库配置：{message}", connectionString + ex.Message);
        }
    }

    /// <summary>
    /// 初始化角色和管理用户
    /// </summary>
    public static async Task InitRoleAndUserAsync(CommandDbContext context, IConfiguration configuration, ILogger<InitDataTask> logger)
    {
        var defaultPassword = configuration.GetValue<string>("Key:DefaultPassword") ?? "Hello.Net";
        SystemRole role = new()
        {
            Name = Const.AdminUser,
            NameValue = Const.AdminUser,
        };
        SystemRole userRole = new()
        {
            Name = Const.User,
            NameValue = Const.User,
        };
        string salt = HashCrypto.BuildSalt();
        SystemUser systemUser = new()
        {
            Id = new Guid("6e2bb78f-fa51-480d-8200-83d488184621"),
            UserName = "admin",
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd(defaultPassword, salt),
            SystemRoles = new List<SystemRole>() { role },
        };

        User user = new()
        {
            Id = new Guid("6e2bb78f-fa51-480d-8200-83d488184621"),
            UserName = "TestUser",
            Email = "TestEmail@dusi.dev",
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd(defaultPassword, salt),
        };

        try
        {
            context.SystemRoles.Add(userRole);
            context.SystemRoles.Add(role);
            context.SystemUsers.Add(systemUser);
            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("初始化角色用户时出错,请确认您的数据库没有数据！{message}", ex.Message);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static async Task UpdateAsync(CommandDbContext context, IConfiguration configuration)
    {
        // 查询库中版本
        SystemConfig? version = await context.SystemConfigs.Where(c => c.Key == Const.Version).FirstOrDefaultAsync();
        if (version == null)
        {
            SystemConfig config = new()
            {
                IsSystem = true,
                Valid = true,
                Key = Const.Version,
                // 版本格式:yyMMdd.编号
                Value = DateTime.UtcNow.ToString("yyMMdd") + ".01"
            };
            context.Add(config);
            await context.SaveChangesAsync();
            version = config;
        }
        // 比对新版本
        string? newVersion = configuration.GetValue<string>(Const.Version);

        if (double.TryParse(newVersion, out double newVersionValue)
            && double.TryParse(version.Value, out double versionValue))
        {
            if (newVersionValue > versionValue)
            {
                // TODO:执行更新方法

                version.Value = newVersionValue.ToString();
                await context.SaveChangesAsync();
            }
        }
        else
        {
            Console.WriteLine("版本格式错误");
        }
    }
}
