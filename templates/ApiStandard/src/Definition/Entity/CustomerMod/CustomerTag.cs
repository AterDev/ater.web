namespace Entity.CustomerMod;
/// <summary>
/// 用户标签
/// </summary>
[Index(nameof(DisplayName))]
[Index(nameof(Key))]
[Module(Modules.Customer)]
public class CustomerTag : EntityBase
{
    /// <summary>
    /// 显示名称
    /// </summary>
    [MaxLength(20)]
    public required string DisplayName { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    [MaxLength(100)]
    public string? Description { get; set; }
    /// <summary>
    /// 唯一标识 
    /// </summary>
    [MaxLength(20)]
    public required string Key { get; set; }

    /// <summary>
    /// 颜色代码:#222222
    /// </summary>
    [MaxLength(20)]
    public string? ColorValue { get; set; }

    public List<CustomerInfo> Customers { get; set; } = [];
    public List<CustomerInfoTag> CustomerInfoTags { get; set; } = [];
}
