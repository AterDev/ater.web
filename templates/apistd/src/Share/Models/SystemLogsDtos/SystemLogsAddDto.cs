using Core.Entities.SystemEntities;
namespace Share.Models.SystemLogsDtos;
/// <summary>
/// 系统日志添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemLogs"/>
public class SystemLogsAddDto
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
    public required string Route { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public required ActionType ActionType { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

}
