namespace Ater.Web.Core.Models;

/// <summary>
/// 数据加基础字段模型
/// </summary>
/// <inheritdoc/>
public abstract class EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedTime { get; set; }
    /// <summary>
    /// 软删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /*
    /// <summary>
    /// 租户标识
    /// </summary>
    [MaxLength(200)]
    public string? Tanant { get; set; }
    */
}

