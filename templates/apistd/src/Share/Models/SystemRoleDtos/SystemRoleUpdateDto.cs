namespace Share.Models.SystemRoleDtos;
/// <summary>
/// 角色更新时请求结构
/// </summary>
//[NgPage("system", "sysrole")]
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemRole"/>
public class SystemRoleUpdateDto
{
    /// <summary>
    /// 角色显示名称
    /// </summary>
    [MaxLength(30)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 角色名，系统标识
    /// </summary>
    public string NameValue { get; set; } = default!;
    /// <summary>
    /// 是否系统内置,系统内置不可删除
    /// </summary>
    public bool? IsSystem { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }

}
