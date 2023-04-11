using Core.Entities.SystemEntities;
namespace Share.Models.SystemConfigDtos;
/// <summary>
/// 系统配置更新时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemConfig"/>
public class SystemConfigUpdateDto
{
    [MaxLength(100)]
    public string Key { get; set; } = default!;
    [MaxLength(100)]
    public string? Value { get; set; }
    [MaxLength(300)]
    public string? Description { get; set; }
    public bool? Valid { get; set; }
    /// <summary>
    /// 是否属于系统配置
    /// </summary>
    public bool? IsSystem { get; set; }
    /// <summary>
    /// 组
    /// </summary>
    public string? GroupName { get; set; }
    
}
