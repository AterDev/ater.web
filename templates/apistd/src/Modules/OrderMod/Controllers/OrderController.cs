using OrderMod.Models.OrderDtos;
namespace OrderMod.Controllers;

/// <summary>
/// 订单
/// </summary>
/// <see cref="OrderMod.Manager.OrderManager"/>
public class OrderController : ClientControllerBase<OrderManager>
{
    private readonly ProductManager _productManager;
    private readonly UserManager _userManager;

    public OrderController(
        IUserContext user,
        ILogger<OrderController> logger,
        OrderManager manager,
        ProductManager productManager,
        UserManager userManager) : base(manager, user, logger)
    {
        _productManager = productManager;
        _userManager = userManager;
    }

    /// <summary>
    /// 订单列表 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<OrderItemDto>>> FilterAsync(OrderFilterDto filter)
    {
        if (!await _user.ExistAsync()) { return NotFound(ErrorMsg.NotFoundUser); }
        filter.UserId = _user.UserId;
        return await manager.FilterAsync(filter);
    }


    /// <summary>
    /// 接收异步通知 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPost("notify")]
    [AllowAnonymous]
    //public async Task<ActionResult> Notify([FromForm] AsyncNotifyModel model)
    //{
    //    var data = Request.Form.ToDictionary(f => f.Key, f => f.Value.First());
    //    if (data == null || !_aliPayService.VerifyNotifySign(data!))
    //    {
    //        return Problem("签名错误");
    //    }

    //    var res = await manager.PayResult(model);
    //    return res ? Ok() : Problem();
    //}

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order?>> GetDetailAsync([FromRoute] Guid id)
    {
        var res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }
}