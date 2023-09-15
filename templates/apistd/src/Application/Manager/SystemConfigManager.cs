using Application.Services;
using Share.Models.SystemConfigDtos;

namespace Application.Manager;
/// <summary>
/// 系统配置
/// </summary>
public class SystemConfigManager : DomainManagerBase<SystemConfig, SystemConfigUpdateDto, SystemConfigFilterDto, SystemConfigItemDto>, IDomainManager<SystemConfig>
{
	private readonly CacheService _cache;
	public SystemConfigManager(
		DataStoreContext storeContext,
		ILogger<SystemConfigManager> logger,
		IUserContext userContext,
		CacheService cache) : base(storeContext, logger)
	{
		_userContext = userContext;
		_cache = cache;
	}

	/// <summary>
	/// 创建待添加实体
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	public async Task<SystemConfig> CreateNewEntityAsync(SystemConfigAddDto dto)
	{
		var entity = dto.MapTo<SystemConfigAddDto, SystemConfig>();
		// other required props
		return await Task.FromResult(entity);
	}

	public override async Task<SystemConfig> UpdateAsync(SystemConfig entity, SystemConfigUpdateDto dto)
	{
		return await base.UpdateAsync(entity, dto);
	}

	public override async Task<PageList<SystemConfigItemDto>> FilterAsync(SystemConfigFilterDto filter)
	{
		Queryable = Queryable
			.WhereNotNull(filter.Key, q => q.Key == filter.Key)
			.WhereNotNull(filter.GroupName, q => q.GroupName == filter.GroupName);

		return await Query.FilterAsync<SystemConfigItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
	}

	/// <summary>
	/// 获取枚举信息 ✅
	/// </summary>
	/// <returns></returns>
	public async Task<Dictionary<string, List<EnumDictionary>>> GetEnumConfigsAsync()
	{
		// TODO:程序启动时更新缓存
		var res = _cache.GetValue<Dictionary<string, List<EnumDictionary>>>(AppConst.EnumCacheName);
		if (res == null || !res.Any())
		{
			var data = EnumHelper.GetAllEnumInfo();
			await _cache.SetValueAsync(AppConst.EnumCacheName, data);
			return data;
		}
		return res;
	}

	/// <summary>
	/// 当前用户所拥有的对象
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public async Task<SystemConfig?> GetOwnedAsync(Guid id)
	{
		var query = Command.Db.Where(q => q.Id == id);
		// 获取用户所属的对象
		// query = query.Where(q => q.User.Id == _userContext.UserId);
		return await query.FirstOrDefaultAsync();
	}

}
