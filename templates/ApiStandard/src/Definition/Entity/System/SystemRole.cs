﻿namespace Entity.System;
/// <summary>
/// 系统角色
/// </summary>
[Index(nameof(Name))]
[Index(nameof(NameValue), IsUnique = true)]
[Module(Modules.System)]
public class SystemRole : IEntityBase
{
    /// <summary>
    /// 角色显示名称
    /// </summary>
    [MaxLength(30)]
    public required string Name { get; set; }
    /// <summary>
    /// 角色名，系统标识
    /// </summary>
    [MaxLength(60)]
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
    public ICollection<SystemUser> Users { get; set; } = [];
    /// <summary>
    /// 中间表
    /// </summary>
    public ICollection<SystemPermissionGroup> PermissionGroups { get; set; } = [];

    /// <summary>
    /// 菜单权限
    /// </summary>
    public ICollection<SystemMenu> Menus { get; set; } = [];
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}
