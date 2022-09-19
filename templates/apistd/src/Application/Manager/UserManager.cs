using Application.IManager;
using Share.Models.UserDtos;

namespace Application.Manager;

public class UserManager : DomainManagerBase<User, UserUpdateDto, UserFilterDto, UserItemDto>, IUserManager
{
    public UserManager(DataStoreContext storeContext) : base(storeContext)
    {
    }

    public override async Task<User> UpdateAsync(User entity, UserUpdateDto dto)
    {
        // TODO:根据实际业务更新
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<UserItemDto>> FilterAsync(UserFilterDto filter)
    {
        // TODO:根据实际业务构建筛选条件
        // if ... Queryable = ...
        return await Query.FilterAsync<UserItemDto>(Queryable);
    }

}
