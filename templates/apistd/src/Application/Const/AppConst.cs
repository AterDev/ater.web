namespace Application.Const;
/// <summary>
/// 应用程序常量
/// </summary>
public static class AppConst
{
    public const string DefaultStateName = "statestore";
    public const string DefaultPubSubName = "pubsub";

    /// <summary>
    /// 管理员policy
    /// </summary>
    public const string AdminUser = "AdminUser";
    /// <summary>
    /// 普通用户policy
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// 版本
    /// </summary>
    public const string Version = "Version";

    /// <summary>
    /// 用户登录缓存前缀
    /// </summary>
    public const string LoginCachePrefix = "Login_";
    /// <summary>
    /// 验证码缓存前缀
    /// </summary>
    public const string VerifyCodeCachePrefix = "VerifyCode_";
}
