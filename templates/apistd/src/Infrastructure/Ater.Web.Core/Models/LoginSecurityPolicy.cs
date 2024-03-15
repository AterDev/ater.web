﻿namespace Ater.Web.Core.Models;
/// <summary>
/// 登录安全策略
/// </summary>
public class LoginSecurityPolicy
{
    /// <summary>
    /// 密码强度
    /// </summary>
    public PasswordLevel PasswordLevel { get; set; } = PasswordLevel.Normal;
    /// <summary>
    /// 是否需要验证码
    /// </summary>
    public bool IsNeedVerifyCode { get; set; }
    /// <summary>
    /// 密码过期时间：月
    /// </summary>
    public int PasswordExpired { get; set; } = 3;

    /// <summary>
    /// 登录失败重试次数
    /// </summary>
    public int LoginRetry { get; set; }

    /// <summary>
    /// 会话数量限制
    /// </summary>
    public SessionLevel SessionLevel { get; set; } = SessionLevel.None;

    /// <summary>
    /// 会话过期时间
    /// </summary>
    public int SessionExpiredSeconds { get; set; } = 1800;
}

/// <summary>
/// 会话限制
/// </summary>
public enum SessionLevel
{
    /// <summary>
    /// 无限制
    /// </summary>
    [Description("无限制")]
    None,
    /// <summary>
    /// 每个客户端，只允许存在一个有效账号状态
    /// </summary>
    [Description("每个客户端，只允许存在一个有效账号状态")]
    OnlyClient,
    /// <summary>
    /// 指在任何客户端，只允许账号在一处生效
    /// </summary>
    [Description("指在任何客户端，只允许账号在一处生效")]
    OnlyOne
}

/// <summary>
/// 密码验证等级
/// </summary>
public enum PasswordLevel
{
    /// <summary>
    /// 最低6位
    /// </summary>
    [Description("最低6位")]
    Simple,
    /// <summary>
    /// 最低8位，要求大小写+数字
    /// </summary>
    [Description("最低8位，要求大小写+数字")]
    Normal,
    /// <summary>
    /// 最低8位，大小写+特殊字符+数字
    /// </summary>
    [Description("最低8位，大小写+特殊字符+数字")]
    Strict
}