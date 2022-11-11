namespace Application.Services;
public class InitDataTask
{
    public static async Task InitDataAsync(IServiceProvider provider)
    {
        CommandDbContext context = provider.GetRequiredService<CommandDbContext>();
        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        ILogger<InitDataTask> logger = loggerFactory.CreateLogger<InitDataTask>();
        string? connectionString = context.Database.GetConnectionString();
        try
        {
            context.Database.Migrate();
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogError("数据库无法连接:" + connectionString);
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
            }
        }
        catch (Exception ex)
        {
            logger.LogError("初始化异常,请检查数据库配置：" + ex.Message);
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
}
