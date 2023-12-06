using OrderMod.Models.ProductDtos;

namespace OrderMod.Manager;
/// <summary>
/// 产品
/// </summary>
public class ProductManager : ManagerBase<Product, ProductUpdateDto, ProductFilterDto, ProductItemDto>
{
    private readonly IUserContext _userContext;
    public ProductManager(
        DataAccessContext<Product> dataContext,
        ILogger<ProductManager> logger,
        IUserContext userContext
        ) : base(dataContext, logger)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Product> CreateNewEntityAsync(ProductAddDto dto)
    {
        Product entity = dto.MapTo<ProductAddDto, Product>();
        return await Task.FromResult(entity);
    }

    public override async Task<Product> UpdateAsync(Product entity, ProductUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<ProductItemDto>> FilterAsync(ProductFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.ProductType, q => q.ProductType == filter.ProductType)
            .WhereNotNull(filter.Name, q => q.Name.Contains(filter.Name!));

        filter.OrderBy = new Dictionary<string, bool>
        {
            ["Sort"] = true
        };

        return await Query.FilterAsync<ProductItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<Order?> BuyProductAsync(Guid productId)
    {
        // 查询产品
        Product? product = await FindAsync(productId);
        if (product == null)
        {
            ErrorMsg = "产品不存在";
            return null;
        }

        // 生成订单
        var order = new Order
        {
            ProductName = product.Name,
            ProductId = product.Id,
            UserId = _userContext.UserId,
            OriginPrice = product.OriginPrice,
            TotalPrice = product.Price,
            Status = OrderStatus.Paid,
        };
        // TODO:其他逻辑
        await SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Product?> GetOwnedAsync(Guid id)
    {
        IQueryable<Product> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

}
