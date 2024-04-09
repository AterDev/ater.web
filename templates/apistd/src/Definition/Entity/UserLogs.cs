﻿namespace Entity;
/// <summary>
/// 用户日志
/// </summary>
[Index(nameof(ActionType))]
[Index(nameof(ActionUserName))]
[Index(nameof(CreatedTime))]
public class UserLogs : IEntityBase
{
    /// <summary>
    /// 操作人名称
    /// </summary>
    [MaxLength(100)]
    public required string ActionUserName { get; set; }

    /// <summary>
    /// 操作对象名称
    /// </summary>
    [MaxLength(100)]
    public required string TargetName { get; set; }

    /// <summary>
    /// 操作路由
    /// </summary>
    [MaxLength(200)]
    public required string Route { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型
    /// </summary>
    public required UserActionType ActionType { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public Guid UserId { get; set; } = default!;

    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }

    public static UserLogs NewLog(string userName, Guid userId, string targetName, UserActionType actionType, string? route = null, string? description = null)
    {
        return new UserLogs
        {
            UserId = userId,
            ActionUserName = userName,
            TargetName = targetName,
            Route = route ?? string.Empty,
            ActionType = actionType,
            Description = description,
        };
    }
}

public enum UserActionType
{
    /// <summary>
    /// 其它
    /// </summary>
    [Description("其它")]
    Else,
    /// <summary>
    /// 登录
    /// </summary>
    [Description("登录")]
    Login,
    /// <summary>
    /// 添加
    /// </summary>
    [Description("添加")]
    Add,
    /// <summary>
    /// 更新
    /// </summary>
    [Description("更新")]
    Update,
    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    Delete,
    /// <summary>
    /// 审查
    /// </summary>
    [Description("审核")]
    Audit,
    /// <summary>
    /// 导入
    /// </summary>
    [Description("导入")]
    Import,
    /// <summary>
    /// 导出
    /// </summary>
    [Description("导出")]
    Export
}
