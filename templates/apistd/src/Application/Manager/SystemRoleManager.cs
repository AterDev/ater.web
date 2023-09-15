using Share.Models.SystemRoleDtos;

namespace Application.Manager;

public class SystemRoleManager : DomainManagerBase<SystemRole, SystemRoleUpdateDto, SystemRoleFilterDto, SystemRoleItemDto>, IDomainManager<SystemRole>
{
	public SystemRoleManager(
		DataStoreContext storeContext,
		ILogger<SystemRoleManager> logger,
		IUserContext userContext) : base(storeContext, logger)
	{
		_userContext = userContext;
	}

	/// <summary>
	/// 创建待添加实体
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	public Task<SystemRole> CreateNewEntityAsync(SystemRoleAddDto dto)
	{
		SystemRole entity = dto.MapTo<SystemRoleAddDto, SystemRole>();
		// other required props
		return Task.FromResult(entity);
	}

	public override async Task<SystemRole> UpdateAsync(SystemRole entity, SystemRoleUpdateDto dto)
	{
		return await base.UpdateAsync(entity, dto);
	}

	public override async Task<PageList<SystemRoleItemDto>> FilterAsync(SystemRoleFilterDto filter)
	{
		Queryable = Queryable
			.WhereNotNull(filter.Name, q => q.Name == filter.Name)
			.WhereNotNull(filter.NameValue, q => q.NameValue == filter.NameValue);
		return await Query.FilterAsync<SystemRoleItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
	}

	/// <summary>
	/// 获取菜单
	/// </summary>
	/// <param name="systemRoles"></param>
	/// <returns></returns>
	public async Task<List<SystemMenu>> GetSystemMenusAsync(List<SystemRole> systemRoles)
	{
		var ids = systemRoles.Select(r => r.Id);
		return await Stores.CommandContext.SystemMenus
			.Where(m => m.Roles.Any(r => ids.Contains(r.Id)))
		.ToListAsync();
	}

	/// <summary>
	/// Set PermissionGroups
	/// </summary>
	/// <param name="current"></param>
	/// <param name="dto"></param>
	/// <returns></returns>
	public async Task<SystemRole?> SetPermissionGroupsAsync(SystemRole current, SystemRoleSetPermissionGroupsDto dto)
	{
		try
		{
			await Stores.CommandContext.Entry(current)
				.Collection(r => r.PermissionGroups)
				.LoadAsync();

			var groups = await Stores.CommandContext.SystemPermissionGroups
				.Where(m => dto.PermissionGroupIds.Contains(m.Id))
				.ToListAsync();
			current.PermissionGroups = groups;
			await Command.SaveChangeAsync();
			return current;
		}
		catch (Exception e)
		{
			_logger.LogError($"set role permission groups failed:{0}", e.Message);
			return null;
		}
	}

	/// <summary>
	/// 获取权限组
	/// </summary>
	/// <param name="systemRoles"></param>
	/// <returns></returns>
	public async Task<List<SystemPermissionGroup>> GetPermissionGroupsAsync(List<SystemRole> systemRoles)
	{
		var ids = systemRoles.Select(r => r.Id);
		return await Stores.CommandContext.SystemPermissionGroups
			.Where(m => m.Roles.Any(r => ids.Contains(r.Id)))
			.ToListAsync();
	}

	/// <summary>
	/// 当前用户所拥有的对象
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public async Task<SystemRole?> GetOwnedAsync(Guid id)
	{
		IQueryable<SystemRole> query = Command.Db.Where(q => q.Id == id);
		// 获取用户所属的对象
		// query = query.Where(q => q.User.Id == _userContext.UserId);
		return await query.FirstOrDefaultAsync();
	}

	/// <summary>
	/// 更新角色菜单
	/// </summary>
	/// <param name="current"></param>
	/// <param name="dto"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public async Task<SystemRole?> SetMenusAsync(SystemRole current, SystemRoleSetMenusDto dto)
	{
		// 更新角色菜单
		try
		{
			await Stores.CommandContext.Entry(current)
				.Collection(r => r.Menus)
				.LoadAsync();

			current.Menus = new List<SystemMenu>();

			var menus = await Stores.CommandContext.SystemMenus
				.Where(m => dto.MenuIds.Contains(m.Id))
				.ToListAsync();
			current.Menus = menus;
			await Command.SaveChangeAsync();
			return current;
		}
		catch (Exception e)
		{
			_logger.LogError($"update role menus failed:{0}", e.Message);
			return default;
		}

	}
}
