using Core.Const;
using Share.Models.BlogDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 博客
/// </summary>
public class BlogController : RestControllerBase<IBlogManager>
{
    private readonly IUserManager _userManager;
    private readonly ICatalogManager _catalogManager;

    public BlogController(
        IUserContext user,
        ILogger<BlogController> logger,
        IBlogManager manager,
        IUserManager userManager,
        ICatalogManager catalogManager
        ) : base(manager, user, logger)
    {
        _userManager = userManager;
        _catalogManager = catalogManager;

    }

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<BlogItemDto>>> FilterAsync(BlogFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Blog>> AddAsync(BlogAddDto dto)
    {
        if (!await _userManager.ExistAsync(dto.UserId))
            return NotFound("不存在的");
        if (!await _user.ExistAsync())
            return NotFound(ErrorMsg.NotFoundUser);
        var entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Blog?>> UpdateAsync([FromRoute] Guid id, BlogUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null) return NotFound(ErrorMsg.NotFoundResource);
        if (current.Catalog.Id != dto.CatalogId)
        {
            var catalog = await _catalogManager.GetCurrentAsync(dto.CatalogId);
            if (catalog == null) return NotFound("不存在的所属目录");
            current.Catalog = catalog;
        }        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Blog?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Blog?>> DeleteAsync([FromRoute] Guid id)
    {
        // TODO:实现删除逻辑,注意删除权限
        var entity = await manager.GetOwnedAsync(id);
        if (entity == null) return NotFound();
        return Forbid();
        // return await manager.DeleteAsync(entity);
    }
}