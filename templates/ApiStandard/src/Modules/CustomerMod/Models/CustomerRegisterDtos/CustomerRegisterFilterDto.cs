using Entity.CustomerMod;
namespace CustomerMod.Models.CustomerRegisterDtos;
/// <summary>
/// 客户登记查询筛选
/// </summary>
/// <see cref="Entity.CustomerMod.CustomerRegister"/>
public class CustomerRegisterFilterDto : FilterBase
{
    /// <summary>
    /// 姓名
    /// </summary>
    [MaxLength(20)]
    public string? Name { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public GenderType? Gender { get; set; }
    /// <summary>
    /// 年龄段
    /// </summary>
    public AgeRange? AgeRange { get; set; }
    /// <summary>
    /// 英语水平
    /// </summary>
    public EnglishLevel? EnglishLevel { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// 微信号/昵称
    /// </summary>
    [MaxLength(100)]
    public string? Weixin { get; set; }
    
}
