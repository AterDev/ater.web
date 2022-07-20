namespace Core.Models;

/// <summary>
/// 数据加基础字段模型
/// </summary>
/// <inheritdoc/>
public class EntityBase
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedTime { get; set; } = DateTimeOffset.UtcNow;
    /// <summary>
    /// 软删除
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}

