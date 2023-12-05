namespace Share.Models.SystemPermissionGroupDtos;

/// <see cref="Entity.System.SystemPermissionGroup"/>
public class SystemPermissionGroupFilterDto : FilterBase
{
    /// <summary>
    /// 权限名称标识
    /// </summary>
    [MaxLength(30)]
    public string? Name { get; set; }
}
