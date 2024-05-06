using Entity.CustomerMod;
namespace CustomerMod.Models.CustomerRegisterDtos;
/// <summary>
/// 客户登记列表元素
/// </summary>
/// <see cref="Entity.CustomerMod.CustomerRegister"/>
public class CustomerRegisterItemDto
{
    /// <summary>
    /// 姓名
    /// </summary>
    [MaxLength(20)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; } = default!;
    /// <summary>
    /// 年龄段
    /// </summary>
    public AgeRange AgeRange { get; set; }
    /// <summary>
    /// 职业 
    /// </summary>
    [MaxLength(50)]
    public string Occupation { get; set; } = default!;
    /// <summary>
    /// 英语水平
    /// </summary>
    public EnglishLevel EnglishLevel { get; set; }
    /// <summary>
    /// 学习目标
    /// </summary>
    [MaxLength(100)]
    public string LearningGoal { get; set; } = default!;
    /// <summary>
    /// 教材倾向:生活/商务/雅思托福
    /// </summary>
    [MaxLength(20)]
    public string PreferredMaterial { get; set; } = default!;
    /// <summary>
    /// 老师偏好
    /// </summary>
    [MaxLength(200)]
    public string TeacherPreference { get; set; } = default!;
    /// <summary>
    /// 试听时间
    /// </summary>
    [MaxLength(300)]
    public string? AvailableTimes { get; set; }
    /// <summary>
    /// 上课时间段
    /// </summary>
    [MinLength(300)]
    public string? ClassSchedule { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = default!;
    /// <summary>
    /// 微信号/昵称
    /// </summary>
    [MaxLength(100)]
    public string Weixin { get; set; } = default!;
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    
}
