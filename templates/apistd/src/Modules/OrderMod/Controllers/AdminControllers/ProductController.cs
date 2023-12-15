using Application;
using OrderMod.Models.ProductDtos;
namespace OrderMod.Controllers.AdminControllers;

/// <summary>
/// 产品
/// </summary>
/// <see cref="OrderMod.Manager.ProductManager"/>
public class ProductController(
    IUserContext user,
    ILogger<ProductController> logger,
    ProductManager manager
        ) : RestControllerBase<ProductManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<ProductItemDto>>> FilterAsync(ProductFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Product>> AddAsync(ProductAddDto dto)
    {
        Product entity = await manager.CreateNewEntityAsync(dto);
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<Product?>> UpdateAsync([FromRoute] Guid id, ProductUpdateDto dto)
    {
        Product? current = await manager.GetCurrentAsync(id);
        if (current == null) { return NotFound(ErrorMsg.NotFoundResource); };
        return await manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> GetDetailAsync([FromRoute] Guid id)
    {
        Product? res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// 删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Product?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        Product? entity = await manager.GetCurrentAsync(id);
        if (entity == null) { return NotFound(); };
        // return Forbid();
        return await manager.DeleteAsync(entity);
    }
}