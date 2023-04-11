using Core.Entities.SystemEntities;
namespace Share.Models.SystemConfigDtos;
/// <summary>
/// 系统配置概要
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemConfig"/>
public class SystemConfigShortDto
{
    [MaxLength(100)]
    public string Key { get; set; } = default!;
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
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
