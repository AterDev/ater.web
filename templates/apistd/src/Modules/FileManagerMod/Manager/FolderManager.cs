using FileManagerMod.Models.FolderDtos;

namespace FileManagerMod.Manager;
/// <summary>
/// 文件夹
/// </summary>
public class FolderManager : ManagerBase<Folder, FolderUpdateDto, FolderFilterDto, FolderItemDto>
{
    public FolderManager(
        DataAccessContext<Folder> dataContext,
        ILogger<FolderManager> logger,
        IUserContext userContext) : base(dataContext, logger)
    {

    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Folder> CreateNewEntityAsync(FolderAddDto dto)
    {
        var entity = dto.MapTo<FolderAddDto, Folder>();
        if (dto.ParentId != null)
        {
            var parent = await FindAsync(dto.ParentId.Value);
            entity.Parent = parent;
            entity.Path = $"{parent!.Path}.{entity.Name}";
        }
        else
        {
            entity.Path = entity.Name;
        }
        return await Task.FromResult(entity);
    }

    public override async Task<Folder> UpdateAsync(Folder entity, FolderUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<FolderItemDto>> FilterAsync(FolderFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Name, q => q.Name == filter.Name)
            .WhereNotNull(filter.ParentId, q => q.ParentId == filter.ParentId);

        return await Query.FilterAsync<FolderItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Folder?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
