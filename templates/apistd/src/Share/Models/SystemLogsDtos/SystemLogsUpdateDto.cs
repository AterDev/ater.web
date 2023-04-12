using Core.Entities.SystemEntities;
namespace Share.Models.SystemLogsDtos;
/// <summary>
/// 系统日志更新时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemLogs"/>
public class SystemLogsUpdateDto
{
    /// <summary>
    /// 操作人名称
    /// </summary>
    [MaxLength(100)]
    public string ActionUserName { get; set; } = default!;
    /// <summary>
    /// 操作对象名称
    /// </summary>
    [MaxLength(100)]
    public string TargetName { get; set; } = default!;
    /// <summary>
    /// 操作路由
    /// </summary>
    [MaxLength(200)]
    public string Route { get; set; } = default!;
    /// <summary>
    /// 操作类型
    /// </summary>
    public ActionType ActionType { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

}
