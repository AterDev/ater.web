using Microsoft.AspNetCore.Mvc;
using Share.Models.SystemMenuDtos;
using Share.Models.SystemRoleDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemRoleManager : IDomainManager<SystemRole>
{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemRole?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemRole> CreateNewEntityAsync(SystemRoleAddDto dto);
    Task<SystemRole?> GetCurrentAsync(Guid id, params string[] navigations);
    Task<SystemRole> AddAsync(SystemRole entity);
    Task<SystemRole> UpdateAsync(SystemRole entity, SystemRoleUpdateDto dto);
    Task<SystemRole?> FindAsync(Guid id);
    Task<TDto?> FindAsync<TDto>(Expression<Func<SystemRole, bool>>? whereExp) where TDto : class;
    Task<List<TDto>> ListAsync<TDto>(Expression<Func<SystemRole, bool>>? whereExp) where TDto : class;
    Task<PageList<SystemRoleItemDto>> FilterAsync(SystemRoleFilterDto filter);
    Task<SystemRole?> DeleteAsync(SystemRole entity, bool softDelete = true);
    Task<bool> ExistAsync(Guid id);
    Task<SystemRole?> UpdateMenusAsync(SystemRole current, SystemRoleUpdateMenusDto dto);
}
