using Share.Models.RolePermissionDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface IRolePermissionManager : IDomainManager<RolePermission, RolePermissionUpdateDto, RolePermissionFilterDto, RolePermissionItemDto>
{
	/// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RolePermission?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<RolePermission> CreateNewEntityAsync(RolePermissionAddDto dto);
}
