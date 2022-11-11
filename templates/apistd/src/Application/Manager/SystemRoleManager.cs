using Share.Models.SystemRoleDtos;

namespace Application.Manager;

public class SystemRoleManager : DomainManagerBase<SystemRole, SystemRoleUpdateDto, SystemRoleFilterDto, SystemRoleItemDto>, ISystemRoleManager
{
    public SystemRoleManager(DataStoreContext storeContext) : base(storeContext)
    {
    }

    public override async Task<SystemRole> UpdateAsync(SystemRole entity, SystemRoleUpdateDto dto)
    {
        // TODO:根据实际业务更新
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemRoleItemDto>> FilterAsync(SystemRoleFilterDto filter)
    {
        // TODO:根据实际业务构建筛选条件
        // if ... Queryable = ...
        return await Query.FilterAsync<SystemRoleItemDto>(Queryable, filter.PageIndex, filter.PageSize);
    }

}
