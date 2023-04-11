using Core.Entities.SystemEntities;
using Share.Models.SystemRoleDtos;
using Share.Models.SystemUserDtos;

namespace Application.Manager;

public class SystemRoleManager : DomainManagerBase<SystemRole, SystemRoleUpdateDto, SystemRoleFilterDto, SystemRoleItemDto>, ISystemRoleManager
{
    public SystemRoleManager(DataStoreContext storeContext) : base(storeContext)
    {
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<SystemRole> CreateNewEntityAsync(SystemRoleAddDto dto)
    {
        var entity = dto.MapTo<SystemRoleAddDto, SystemRole>();
        // other required props
        return Task.FromResult(entity);
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

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemRole?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // TODO:获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
