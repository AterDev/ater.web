using Entity.CustomerMod;
namespace CustomerMod.Models.CustomerTagDtos;
/// <summary>
/// 用户标签查询筛选
/// </summary>
/// <see cref="Entity.CustomerMod.CustomerTag"/>
public class CustomerTagFilterDto : FilterBase
{
    /// <summary>
    /// 显示名称
    /// </summary>
    [MaxLength(20)]
    public string? DisplayName { get; set; }
    /// <summary>
    /// 唯一标识 
    /// </summary>
    [MaxLength(20)]
    public string? Key { get; set; }


}
