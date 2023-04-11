using Application.IManager;
using Share.Models.RolePermissionDtos;

namespace Application.Manager;

public class RolePermissionManager : DomainManagerBase<RolePermission, RolePermissionUpdateDto, RolePermissionFilterDto, RolePermissionItemDto>, IRolePermissionManager
{

    private readonly IUserContext _userContext;
    public RolePermissionManager(
        DataStoreContext storeContext, 
        IUserContext userContext) : base(storeContext)
    {

        _userContext = userContext;
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<RolePermission> CreateNewEntityAsync(RolePermissionAddDto dto)
    {
        var entity = dto.MapTo<RolePermissionAddDto, RolePermission>();
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<RolePermission> UpdateAsync(RolePermission entity, RolePermissionUpdateDto dto)
    {
      return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<RolePermissionItemDto>> FilterAsync(RolePermissionFilterDto filter)
    {
        /*
        Queryable = Queryable
        */
        // TODO: other filter conditions
        return await Query.FilterAsync<RolePermissionItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RolePermission?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // TODO:获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
