namespace CMSMod.Models.CatalogDtos;
/// <summary>
/// 目录概要
/// </summary>
/// <inheritdoc cref="Catalog"/>
public class CatalogShortDto
{
    /// <summary>
    /// 目录名称
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 层级
    /// </summary>
    public short Level { get; set; } = 0;
    /// <summary>
    /// 父目录
    /// </summary>
    public Catalog? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;

}
