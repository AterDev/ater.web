using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;
/// <summary>
/// 网站配置
/// </summary>
[Index(nameof(Key))]
public class WebConfig : EntityBase
{
    public WebConfig(string key, string value = "")
    {
        Key = key;
        Value = value;
    }
    public WebConfig()
    {
    }

    [MaxLength(100)]
    public required string Key { get; init; }
    [MaxLength(100)]
    public string Value { get; init; } = string.Empty;
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
