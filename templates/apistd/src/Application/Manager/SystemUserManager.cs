using Core.Entities.SystemEntities;
using Share.Models.SystemMenuDtos;
using Share.Models.SystemUserDtos;

namespace Application.Manager;

public class SystemUserManager : DomainManagerBase<SystemUser, SystemUserUpdateDto, SystemUserFilterDto, SystemUserItemDto>, ISystemUserManager
{
    public SystemUserManager(DataStoreContext storeContext) : base(storeContext)
    {
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<SystemUser> CreateNewEntityAsync(SystemUserAddDto dto)
    {
        var entity = dto.MapTo<SystemUserAddDto, SystemUser>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<SystemUser> UpdateAsync(SystemUser entity, SystemUserUpdateDto dto)
    {
        // TODO:根据实际业务更新
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemUserItemDto>> FilterAsync(SystemUserFilterDto filter)
    {
        // TODO:根据实际业务构建筛选条件
        // if ... Queryable = ...
        return await Query.FilterAsync<SystemUserItemDto>(Queryable, filter.PageIndex, filter.PageSize);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemUser?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // TODO:获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }
}
