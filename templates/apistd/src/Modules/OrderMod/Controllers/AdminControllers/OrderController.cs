using OrderMod.Models.OrderDtos;
namespace OrderMod.Controllers.AdminControllers;

/// <summary>
/// 订单
/// </summary>
/// <see cref="OrderMod.Manager.OrderManager"/>
public class OrderController : RestControllerBase<OrderManager>
{
    private readonly ProductManager _productManager;
    private readonly UserManager _userManager;

    public OrderController(
        IUserContext user,
        ILogger<OrderController> logger,
        OrderManager manager,
        ProductManager productManager,
        UserManager userManager
        ) : base(manager, user, logger)
    {
        _productManager = productManager;
        _userManager = userManager;

    }

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<OrderItemDto>>> FilterAsync(OrderFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 更新订单状态 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Order?>> UpdateAsync([FromRoute] Guid id, OrderUpdateDto dto)
    {
        var current = await manager.GetCurrentAsync(id);
        if (current == null) { return NotFound(ErrorMsg.NotFoundResource); };
        return await manager.UpdateAsync(current, dto);
    }

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