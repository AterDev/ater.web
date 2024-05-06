using Entity.SystemMod;
using SystemMod.Models.SystemLogsDtos;

namespace SystemMod.Manager;
/// <summary>
/// 系统日志
/// </summary>
public class SystemLogsManager(
    DataAccessContext<SystemLogs> dataContext,
    ILogger<SystemLogsManager> logger,
    IUserContext userContext) : ManagerBase<SystemLogs, SystemLogsUpdateDto, SystemLogsFilterDto, SystemLogsItemDto>(dataContext, logger)
{
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<SystemLogs> CreateNewEntityAsync(SystemLogsAddDto dto)
    {
        SystemLogs entity = dto.MapTo<SystemLogsAddDto, SystemLogs>();
        entity.SystemUserId = _userContext.UserId;
        return await Task.FromResult(entity);
    }

    public override async Task<SystemLogs> UpdateAsync(SystemLogs entity, SystemLogsUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemLogsItemDto>> FilterAsync(SystemLogsFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.ActionUserName, q => q.ActionUserName == filter.ActionUserName)
            .WhereNotNull(filter.TargetName, q => q.TargetName == filter.TargetName)
            .WhereNotNull(filter.ActionType, q => q.ActionType == filter.ActionType);

        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        {
            // 包含今天
            var endDate = filter.EndDate.Value.AddDays(1);
            Queryable = Queryable.Between(q => q.CreatedTime, filter.StartDate.Value, endDate);
        }
        return await Query.FilterAsync<SystemLogsItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemLogs?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemLogs> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }
}
