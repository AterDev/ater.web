using Application.IManager;
using Share.Models.TagsDtos;

namespace Application.Manager;

public class TagsManager : DomainManagerBase<Tags, TagsUpdateDto, TagsFilterDto, TagsItemDto>, ITagsManager
{

    private readonly IUserContext _userContext;
    public TagsManager(
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
    public Task<Tags> CreateNewEntityAsync(TagsAddDto dto)
    {
        var entity = dto.MapTo<TagsAddDto, Tags>();
        Command.Db.Entry(entity).Property("UserId").CurrentValue = _userContext.UserId!.Value;
        // or entity.UserId = _userContext.UserId!.Value;
        // other required props
        return Task.FromResult(entity);
    }

    public override async Task<Tags> UpdateAsync(Tags entity, TagsUpdateDto dto)
    {
      return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<TagsItemDto>> FilterAsync(TagsFilterDto filter)
    {
        /*
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.UserId, q => q.User.Id == filter.UserId)
        */
        // TODO: other filter conditions
        return await Query.FilterAsync<TagsItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Tags?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // TODO:获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
