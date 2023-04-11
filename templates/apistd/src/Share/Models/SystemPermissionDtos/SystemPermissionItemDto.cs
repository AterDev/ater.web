using Core.Entities.SystemEntities;
namespace Share.Models.SystemPermissionDtos;
/// <summary>
/// 权限列表元素
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemPermission"/>
public class SystemPermissionItemDto
{
    [MaxLength(30)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 权限路径
    /// </summary>
    [MaxLength(200)]
    public string? PermissionPath { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    
}
