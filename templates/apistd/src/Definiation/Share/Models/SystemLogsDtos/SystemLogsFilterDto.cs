using Entity.SystemEntities;
namespace Share.Models.SystemLogsDtos;
/// <summary>
/// 系统日志查询筛选
/// </summary>
/// <see cref="Entity.SystemEntities.SystemLogs"/>
public class SystemLogsFilterDto : FilterBase
{
    /// <summary>
    /// 操作人名称
    /// </summary>
    [MaxLength(100)]
    public string? ActionUserName { get; set; }
    /// <summary>
    /// 操作对象名称
    /// </summary>
    [MaxLength(100)]
    public string? TargetName { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public ActionType? ActionType { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateOnly? StartDate { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateOnly? EndDate { get; set; }

    public Guid? SystemUserId { get; set; }

}
