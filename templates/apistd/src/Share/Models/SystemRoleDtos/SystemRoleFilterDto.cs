using Core.Entities;
namespace Share.Models.SystemRoleDtos;
/// <summary>
/// 角色查询筛选
/// </summary>
public class SystemRoleFilterDto : FilterBase
{
    /// <summary>
    /// 角色显示名称
    /// </summary>
    [MaxLength(30)]
    public string? Name { get; set; }
    /// <summary>
    /// 角色名，系统标识
    /// </summary>
    public string? NameValue { get; set; }
    /// <summary>
    /// 是否系统内置,系统内置不可删除
    /// </summary>
    public bool? IsSystem { get; set; }
    
}
