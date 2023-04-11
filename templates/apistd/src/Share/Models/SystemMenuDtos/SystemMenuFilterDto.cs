using Core.Entities.SystemEntities;
namespace Share.Models.SystemMenuDtos;
/// <summary>
/// 系统菜单查询筛选
/// </summary>
/// <inheritdoc cref="Core.Entities.SystemEntities.SystemMenu"/>
public class SystemMenuFilterDto : FilterBase
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    [MaxLength(60)]
    public string? Name { get; set; }
    /// <summary>
    /// 菜单路径
    /// </summary>
    [MaxLength(100)]
    public string? Path { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    [MaxLength(30)]
    public string? Icon { get; set; }
    public Guid? ParentId { get; set; }
    /// <summary>
    ///  是否有效
    /// </summary>
    public bool? IsValid { get; set; }
    /// <summary>
    /// 权限编码
    /// </summary>
    [MaxLength(50)]
    public string? AccessCode { get; set; }
    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType? MenuType { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool? Hidden { get; set; }
    
}
