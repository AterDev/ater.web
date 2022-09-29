using Core.Entities;
namespace Share.Models.SystemRoleDtos;
/// <summary>
/// 角色概要
/// </summary>
public class SystemRoleShortDto
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
    public bool IsSystem { get; set; } = default!;
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }
    public Guid Id { get; set; } = default!;
    public DateTimeOffset CreatedTime { get; set; } = default!;
    public DateTimeOffset UpdatedTime { get; set; } = default!;
    /// <summary>
    /// 软删除
    /// </summary>
    public bool IsDeleted { get; set; } = default!;
    
}
