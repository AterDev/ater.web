using Application.IManager;
using Share.Models.RoleDtos;

namespace Application.Manager;

public class RoleManager : DomainManagerBase<Role, RoleUpdateDto, RoleFilterDto, RoleItemDto>, IRoleManager
{
    public RoleManager(DataStoreContext storeContext) : base(storeContext)
    {
    }

    public override async Task<Role> UpdateAsync(Role entity, RoleUpdateDto dto)
    {
        // TODO:根据实际业务更新
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<RoleItemDto>> FilterAsync(RoleFilterDto filter)
    {
        // TODO:根据实际业务构建筛选条件
        // if ... Queryable = ...
        return await Query.FilterAsync<RoleItemDto>(Queryable);
    }

}
