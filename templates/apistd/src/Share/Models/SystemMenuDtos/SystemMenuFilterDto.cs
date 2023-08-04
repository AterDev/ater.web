using Entity.SystemEntities;
namespace Share.Models.SystemMenuDtos;
/// <summary>
/// 系统菜单查询筛选
/// </summary>
/// <inheritdoc cref="Entity.SystemEntities.SystemMenu"/>
public class SystemMenuFilterDto : FilterBase
{
    public Guid? ParentId { get; set; }
}
