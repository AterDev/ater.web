using Entity.CustomerMod;
namespace CustomerMod.Models.CustomerRegisterDtos;
/// <summary>
/// 客户登记更新时请求结构
/// </summary>
/// <see cref="Entity.CustomerMod.CustomerRegister"/>
public class CustomerRegisterUpdateDto
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
    /// 职业 
    /// </summary>
    [MaxLength(50)]
    public string? Occupation { get; set; }
    /// <summary>
    /// 英语水平
    /// </summary>
    public EnglishLevel? EnglishLevel { get; set; }
    /// <summary>
    /// 学习目标
    /// </summary>
    [MaxLength(100)]
    public string? LearningGoal { get; set; }
    /// <summary>
    /// 提升部分:听力/口语/阅读/写作
    /// </summary>
    public string[]? ImproveAreas { get; set; }
    /// <summary>
    /// 教材倾向:生活/商务/雅思托福
    /// </summary>
    [MaxLength(20)]
    public string? PreferredMaterial { get; set; }
    /// <summary>
    /// 老师偏好
    /// </summary>
    [MaxLength(200)]
    public string? TeacherPreference { get; set; }
    /// <summary>
    /// 试听时间
    /// </summary>
    [MaxLength(300)]
    public string? AvailableTimes { get; set; }
    /// <summary>
    /// 上课时间段
    /// </summary>
    [MaxLength(300)]
    public string? ClassSchedule { get; set; }
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
