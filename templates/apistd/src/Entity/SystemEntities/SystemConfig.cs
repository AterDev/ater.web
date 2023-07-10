using Microsoft.EntityFrameworkCore;

namespace Entity.SystemEntities;
/// <summary>
/// 系统配置
/// </summary>
[Index(nameof(Key))]
public class SystemConfig : EntityBase
{
    public SystemConfig(string key, string value = "")
    {
        Key = key;
        Value = value;
    }
    public SystemConfig()
    {
    }

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
