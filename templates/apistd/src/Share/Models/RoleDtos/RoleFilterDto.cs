namespace Share.Models.RoleDtos;
/// <summary>
/// 角色查询筛选
/// </summary>
public class RoleFilterDto : FilterBase
{
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
