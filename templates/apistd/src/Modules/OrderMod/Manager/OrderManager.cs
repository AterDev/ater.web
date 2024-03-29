using Application;

using OrderMod.Models.OrderDtos;

namespace OrderMod.Manager;
/// <summary>
/// 订单
/// </summary>
public class OrderManager(
    DataAccessContext<Order> dataContext,
    ILogger<OrderManager> logger,
    IUserContext userContext,
    ProductManager productManager) : ManagerBase<Order, OrderUpdateDto, OrderFilterDto, OrderItemDto>(dataContext, logger)
{
    private readonly ProductManager _productManager = productManager;
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <returns></returns>
    public async Task<Order?> CreateNewEntityAsync(OrderAddDto dto)
    {
        Product? product = await _productManager.GetCurrentAsync(dto.ProductId);
        return product != null
            ? new Order
            {
                Product = product,
                UserId = _userContext.UserId,
                ProductName = product.Name,
                DiscountCode = dto.DiscountCode,
                OriginPrice = product.OriginPrice,
                TotalPrice = product.Price,
                Status = OrderStatus.UnPaid
            }
            : default;
    }

    public override async Task<Order> UpdateAsync(Order entity, OrderUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<OrderItemDto>> FilterAsync(OrderFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.OrderNumber, q => q.OrderNumber == filter.OrderNumber)
            .WhereNotNull(filter.ProductId, q => q.Product.Id == filter.ProductId)
            .WhereNotNull(filter.UserId, q => q.User.Id == filter.UserId)
            .WhereNotNull(filter.Status, q => q.Status == filter.Status);

        return await Query.FilterAsync<OrderItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /*/// <summary>
    /// 异步通知 支付结果
    /// </summary>
    public async Task<bool> PayResult(AsyncNotifyModel model)
    {
        return false;
    }*/

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Order?> GetOwnedAsync(Guid id)
    {
        IQueryable<Order> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }
}
