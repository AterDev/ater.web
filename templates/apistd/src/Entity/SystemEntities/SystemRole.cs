﻿namespace Entity.SystemEntities;
/// <summary>
/// 系统角色
/// </summary>
[Index(nameof(Name))]
[Index(nameof(NameValue), IsUnique = true)]
public class SystemRole : EntityBase
{
    /// <summary>
    /// 角色显示名称
    /// </summary>
    [MaxLength(30)]
    public required string Name { get; set; }
    /// <summary>
    /// 角色名，系统标识
    /// </summary>
    public required string NameValue { get; set; } = string.Empty;
    /// <summary>
    /// 是否系统内置,系统内置不可删除
    /// </summary>
    public bool IsSystem { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }
    public ICollection<SystemUser> Users { get; set; } = new List<SystemUser>();
    /// <summary>
    /// 中间表
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    /// <summary>
    /// 权限
    /// </summary>
    public ICollection<SystemPermission> Permissions { get; set; } = new List<SystemPermission>();
    /// <summary>
    /// 菜单权限
    /// </summary>
    public ICollection<SystemMenu> Menus { get; set; } = new List<SystemMenu>();

}
