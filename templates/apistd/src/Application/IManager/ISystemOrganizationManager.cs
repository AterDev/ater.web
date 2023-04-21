using Share.Models.SystemOrganizationDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemOrganizationManager :IDomainManager<SystemOrganization>{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemOrganization?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemOrganization> CreateNewEntityAsync(SystemOrganizationAddDto dto);
    Task<SystemOrganization?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemOrganization> AddAsync(SystemOrganization entity);
    Task<SystemOrganization> UpdateAsync(SystemOrganization entity, SystemOrganizationUpdateDto dto);
    Task<SystemOrganization?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemOrganization, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemOrganization, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemOrganizationItemDto>> FilterAsync(SystemOrganizationFilterDto filter);
    Task<SystemOrganization?> DeleteAsync(SystemOrganization entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
}
