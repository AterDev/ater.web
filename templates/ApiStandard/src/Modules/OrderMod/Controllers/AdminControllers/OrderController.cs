using Application;
using Entity.OrderMod;
using OrderMod.Models.OrderDtos;
namespace OrderMod.Controllers.AdminControllers;

/// <summary>
/// 订单
/// </summary>
/// <see cref="OrderMod.Manager.OrderManager"/>
public class OrderController(
    IUserContext user,
    ILogger<OrderController> logger,
    OrderManager manager
        ) : RestControllerBase<OrderManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<OrderItemDto>>> FilterAsync(OrderFilterDto filter)
    {
        return await _manager.FilterAsync(filter);
    }

    /// <summary>
    /// 更新订单状态 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<Order?>> UpdateAsync([FromRoute] Guid id, OrderUpdateDto dto)
    {
        Order? current = await _manager.GetCurrentAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        };
        return await _manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order?>> GetDetailAsync([FromRoute] Guid id)
    {
        Order? res = await _manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

}