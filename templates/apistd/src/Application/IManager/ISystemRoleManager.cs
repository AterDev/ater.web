using Core.Entities.SystemEntities;
using Share.Models.SystemRoleDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemRoleManager : IDomainManager<SystemRole, SystemRoleUpdateDto, SystemRoleFilterDto, SystemRoleItemDto>
{
    Task<SystemRole> CreateNewEntityAsync(SystemRoleAddDto dto);

    // TODO: 定义业务方法
    Task<SystemRole?> GetOwnedAsync(Guid id);
}
