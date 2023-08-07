using Share.Models.SystemMenuDtos;

namespace Application.Manager;
/// <summary>
/// 系统菜单
/// </summary>
public class SystemMenuManager : DomainManagerBase<SystemMenu, SystemMenuUpdateDto, SystemMenuFilterDto, SystemMenuItemDto>, ISystemMenuManager
{

    public SystemMenuManager(
        DataStoreContext storeContext,
        ILogger<SystemMenuManager> logger,
        IUserContext userContext) : base(storeContext, logger)
    {

        _userContext = userContext;
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<SystemMenu> CreateNewEntityAsync(SystemMenuAddDto dto)
    {
        var entity = dto.MapTo<SystemMenuAddDto, SystemMenu>();
        if (dto.ParentId != null)
        {
            entity.ParentId = dto.ParentId.Value;
        }

        return await Task.FromResult(entity);
    }

    /// <summary>
    /// 菜单同步
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    public async Task<bool> SyncSystemMenusAsync(List<SystemMenuSyncDto> menus)
    {
        // 查询当前菜单内容
        var currentMenus = await Command.ListAsync();
        var flatMenus = FlatTree(menus);

        var accessCodes = flatMenus.Select(m => m.AccessCode).ToList();
        // 获取需要被删除的
        var needDeleteMenus = currentMenus.Where(m => !accessCodes.Contains(m.AccessCode)).ToList();
        if (needDeleteMenus.Any())
        {
            Command.RemoveRange(needDeleteMenus);
            currentMenus = currentMenus.Except(needDeleteMenus).ToList();
        }

        // 菜单新增与更新
        foreach (var menu in flatMenus)
        {

            if (currentMenus.Any(c => c.AccessCode == menu.AccessCode))
            {
                var index = currentMenus.FindIndex(m => m.AccessCode.Equals(menu.AccessCode));
                if (currentMenus[index].Name != menu.Name)
                {
                    currentMenus[index].Name = menu.Name;
                    Command.Update(currentMenus[index]);
                }
            }
            else
            {
                if (menu.Parent != null)
                {
                    var parent = currentMenus.Where(c => c.AccessCode == menu.Parent.AccessCode).FirstOrDefault();
                    if (parent != null)
                    {
                        menu.Parent = parent;
                    }
                }
                await Command.CreateAsync(menu);
            }
        }
        return await Command.SaveChangeAsync() > 0;
    }

    /// <summary>
    /// flat tree 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private List<SystemMenu> FlatTree(List<SystemMenuSyncDto> list, SystemMenu? parent = null)
    {
        var res = new List<SystemMenu>();
        foreach (var item in list)
        {
            if (item.Children.Any())
            {
                var menu = new SystemMenu
                {
                    Name = item.Name,
                    AccessCode = item.AccessCode,
                    MenuType = (MenuType)item.MenuType,
                    Parent = parent,
                };
                res.Add(menu);
                var children = FlatTree(item.Children, menu);
                res.AddRange(children);
            }
            else
            {
                var menu = new SystemMenu
                {
                    Name = item.Name,
                    AccessCode = item.AccessCode,
                    MenuType = (MenuType)item.MenuType,
                    Parent = parent,
                };
                res.Add(menu);
            }
        }
        return res;
    }

    public override async Task<SystemMenu> UpdateAsync(SystemMenu entity, SystemMenuUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public new async Task<PageList<SystemMenu>> FilterAsync(SystemMenuFilterDto filter)
    {
        List<SystemMenu>? menus;
        if (filter.RoleId != null)
        {
            menus = await Queryable.Where(q => q.Roles.Any(r => r.Id == filter.RoleId))
                .ToListAsync();
        }
        else if (filter.ParentId != null)
        {
            menus = await Queryable.Where(q => q.Parent != null && q.ParentId == filter.ParentId)
                .ToListAsync();
        }
        else
        {
            menus = await Queryable.AsNoTracking()
            .OrderByDescending(t => t.Sort)
                .ThenByDescending(t => t.CreatedTime)
            .Skip((filter.PageIndex - 1) * filter.PageIndex)
            .Take(filter.PageSize)
            .ToListAsync();
            menus = menus.BuildTree();
        }

        return new PageList<SystemMenu>()
        {
            Data = menus,
            PageIndex = filter.PageIndex
        };
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemMenu?> GetOwnedAsync(Guid id)
    {
        var query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
