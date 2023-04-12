using Share.Models.SystemUserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemUserManager : IDomainManager<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>
{
    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SystemUser?> GetOwnedAsync(Guid id);

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto);
}
