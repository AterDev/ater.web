using Application;
using OrderMod.Models.ProductDtos;
namespace OrderMod.Controllers;

/// <summary>
/// 产品
/// </summary>
/// <see cref="OrderMod.Manager.ProductManager"/>
public class ProductController(
    IUserContext user,
    ILogger<ProductController> logger,
    ProductManager manager
        ) : ClientControllerBase<ProductManager>(manager, user, logger)
{

    /// <summary>
    /// 产品列表 ✅
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<ProductItemDto>>> FilterAsync()
    {
        return await manager.ListAsync<ProductItemDto>();
    }

    /// <summary>
    /// 购买产品 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("buy/{id}")]
    public async Task<ActionResult<Order>> BuyProductAsync([FromRoute] Guid id)
    {
        Order? res = await manager.BuyProductAsync(id);
        return res == null ? Problem(manager.ErrorMsg) : (ActionResult<Order>)res;
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
}