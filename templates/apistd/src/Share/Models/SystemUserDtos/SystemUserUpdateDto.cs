using Entity.SystemEntities;

namespace Share.Models.SystemUserDtos;
/// <summary>
/// 系统用户更新时请求结构
/// </summary>
/// <inheritdoc cref="Entity.SystemEntities.SystemUser"/>
public class SystemUserUpdateDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(30)]
    public string UserName { get; set; } = default!;
    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(30)]
    public string? RealName { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// 头像url
    /// </summary>
    [MaxLength(200)]
    public string? Avatar { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public Sex? Sex { get; set; }

}
