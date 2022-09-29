namespace Application.Services;
public class InitDataTask
{
    public static async Task InitDataAsync(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<CommandDbContext>();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<InitDataTask>();

        var connectionString = context.Database.GetConnectionString();
        logger.LogInformation("当前数据库:" + connectionString);

        try
        {
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogError("数据库无法连接，请先配置数据库！");
            }
            else
            {
                // 判断是否初始化
                var role = await context.Roles.SingleOrDefaultAsync(r => r.Name.ToLower() == "admin");
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
        var role = new SystemRole()
        {
            Name = "Admin"
        };
        var userRole = new SystemRole()
        {
            Name = "User"
        };
        var salt = HashCrypto.BuildSalt();
        var user = new SystemUser()
        {
            UserName = "admin",
            PasswordSalt = salt,
            PasswordHash = HashCrypto.GeneratePwd("123456", salt),
            SystemRoles = new List<SystemRole>() { role },
        };
        context.Roles.Add(userRole);
        context.Roles.Add(role);
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
}
