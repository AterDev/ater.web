namespace Share.Models.SystemConfigDtos;
/// <summary>
/// 系统配置添加时请求结构
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemConfig"/>
public class SystemConfigAddDto
{
    [MaxLength(100)]
    public required string Key { get; set; }
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;
    [MaxLength(300)]
    public string? Description { get; set; }
    public bool Valid { get; set; } = true;
    /// <summary>
    /// 是否属于系统配置
    /// </summary>
    public bool IsSystem { get; set; }
    /// <summary>
    /// 组
    /// </summary>
    public string? GroupName { get; set; }

}
