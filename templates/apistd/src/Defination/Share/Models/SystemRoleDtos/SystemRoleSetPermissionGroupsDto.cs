﻿namespace Share.Models.SystemRoleDtos;

/// <summary>
/// 菜单更新
/// </summary>
/// <see cref="Entity.SystemEntities.SystemRole"/>
public class SystemRoleSetPermissionGroupsDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 菜单Id集合
    /// </summary>
    public List<Guid> PermissionGroupIds { get; set; } = new();
}