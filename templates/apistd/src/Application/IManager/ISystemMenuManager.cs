using Share.Models.SystemMenuDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemMenuManager : IDomainManager<SystemMenu, SystemMenuUpdateDto, SystemMenuFilterDto, SystemMenuItemDto>
{
	/// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemMenu?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemMenu> CreateNewEntityAsync(SystemMenuAddDto dto);
}
