using Core.Entities.SystemEntities;
using Share.Models.SystemUserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemUserManager : IDomainManager<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>
{
    Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto);

    // TODO: 定义业务方法
    Task<SystemUser?> GetOwnedAsync(Guid id);
}
