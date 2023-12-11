using Application;
using OrderMod.Models.OrderDtos;
namespace OrderMod.Controllers;

/// <summary>
/// 订单
/// </summary>
/// <see cref="OrderMod.Manager.OrderManager"/>
public class OrderController(
    IUserContext user,
    ILogger<OrderController> logger,
    OrderManager manager) : ClientControllerBase<OrderManager>(manager, user, logger)
{

    /// <summary>
    /// 订单列表 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<OrderItemDto>>> FilterAsync(OrderFilterDto filter)
    {
        filter.UserId = _user.UserId;
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order?>> GetDetailAsync([FromRoute] Guid id)
    {
        Order? res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }
}