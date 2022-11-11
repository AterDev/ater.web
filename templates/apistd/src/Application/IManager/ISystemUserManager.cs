using Share.Models.SystemUserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface ISystemUserManager : IDomainManager<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>
{
    // TODO: 定义业务方法
}
