using OrderMod.Models.ProductDtos;
namespace OrderMod.Controllers;

/// <summary>
/// 产品
/// </summary>
/// <see cref="OrderMod.Manager.ProductManager"/>
public class ProductController : ClientControllerBase<ProductManager>
{

    public ProductController(
        IUserContext user,
        ILogger<ProductController> logger,
        ProductManager manager
        ) : base(manager, user, logger)
    {

    }

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
        var res = await manager.BuyProductAsync(id);
        if (res == null)
        {
            return Problem(manager.ErrorMsg);
        }
        return res;
    }

    /// <summary>
    /// 获取用户贡献值 ✅
    /// </summary>
    /// <returns></returns>
    [HttpGet("getContribution")]
    public async Task<int> GetContributionAsync()
    {
        var res = await manager.GetContributionAsync(_user.UserId!.Value);
        return (int)res;
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }
}