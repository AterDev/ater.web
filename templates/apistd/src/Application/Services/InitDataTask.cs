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
        var configuration = provider.GetRequiredService<IConfiguration>();

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
                SystemRole? role = await context.SystemRoles.SingleOrDefaultAsync(r => r.Name.ToLower() == "admin");
                if (role == null)
                {
                    logger.LogInformation("初始化数据");
                    await InitRoleAndUserAsync(context);
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
    public static async Task InitRoleAndUserAsync(CommandDbContext context)
    {
        SystemRole role = new()
        {
            Name = "Admin"
        };
        SystemRole userRole = new()
        {
            Name = "User"
        };
        string salt = HashCrypto.BuildSalt();
        SystemUser user = new()
        {
            UserName = "admin",
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd("123456", salt),
            SystemRoles = new List<SystemRole>() { role },
        };
        _ = context.SystemRoles.Add(userRole);
        _ = context.SystemRoles.Add(role);
        _ = context.SystemUsers.Add(user);
        _ = await context.SaveChangesAsync();
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
        var version = await context.WebConfigs.Where(c => c.Key == Const.Version).FirstOrDefaultAsync();
        if (version == null)
        {
            var config = new WebConfig
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
        var newVersion = configuration.GetValue<string>(Const.Version);

        if (double.TryParse(newVersion, out var newVersionValue)
            && double.TryParse(version.Value, out var versionValue))
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
