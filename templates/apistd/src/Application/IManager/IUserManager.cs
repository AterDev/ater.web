using Share.Models.UserDtos;

namespace Application.IManager;
/// <summary>
/// 定义实体业务接口规范
/// </summary>
public interface IUserManager : IDomainManager<User, UserUpdateDto, UserFilterDto, UserItemDto>
{
	// TODO: 定义业务方法
}
