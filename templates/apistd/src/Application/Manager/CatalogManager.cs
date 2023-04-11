using Application.IManager;
using Share.Models.CatalogDtos;

namespace Application.Manager;

public class CatalogManager : DomainManagerBase<Catalog, CatalogUpdateDto, CatalogFilterDto, CatalogItemDto>, ICatalogManager
{

    private readonly IUserContext _userContext;
    public CatalogManager(
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
    public Task<Catalog> CreateNewEntityAsync(CatalogAddDto dto)
    {
        var entity = dto.MapTo<CatalogAddDto, Catalog>();
        Command.Db.Entry(entity).Property("UserId").CurrentValue = _userContext.UserId!.Value;
        // or entity.UserId = _userContext.UserId!.Value;
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<Catalog> UpdateAsync(Catalog entity, CatalogUpdateDto dto)
    {
      return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<CatalogItemDto>> FilterAsync(CatalogFilterDto filter)
    {
        /*
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.UserId, q => q.User.Id == filter.UserId)
        */
        // TODO: other filter conditions
        return await Query.FilterAsync<CatalogItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Catalog?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // TODO:获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
