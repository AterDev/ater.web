using Entity.CustomerMod;
namespace CustomerMod.Models.CustomerTagDtos;
/// <summary>
/// 用户标签概要
/// </summary>
/// <see cref="Entity.CustomerMod.CustomerTag"/>
public class CustomerTagShortDto
{
    /// <summary>
    /// 显示名称
    /// </summary>
    [MaxLength(20)]
    public string DisplayName { get; set; } = default!;
    /// <summary>
    /// 说明
    /// </summary>
    [MaxLength(100)]
    public string? Description { get; set; }
    /// <summary>
    /// 唯一标识 
    /// </summary>
    [MaxLength(20)]
    public string Key { get; set; } = default!;
    /// <summary>
    /// 颜色代码:#222222
    /// </summary>
    [MaxLength(20)]
    public string? ColorValue { get; set; }

}
