using Application.Implement;
using Application.Interface;
using Application.Manager;
using Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Http.API.Infrastructure;

[ApiController]
[Route("api/[controller]")]
[Authorize("User")]
public class RestApiBase<TManager, TEntity, TFilter, TUpdate> : RestControllerBase, IRestAPI<TEntity, TFilter, TUpdate>
    where TEntity : EntityBase
    where TFilter : FilterBase
    where TManager : DomainManagerBase<TEntity>
{
    protected readonly ILogger _logger;
    protected readonly TManager manager;
    protected readonly IUserContext _user;
    protected readonly DataStoreContext _storeContext;

    public RestApiBase(IUserContext user, ILogger logger, TManager manager, DataStoreContext storeContext)
    {
        _user = user;
        this.manager = manager;
        _logger = logger;
        _storeContext = storeContext;
    }

    public async Task<ActionResult<TEntity>> AddAsync<TAdd>(TAdd form)
    {
        var entity = form.MapTo<TAdd, TEntity>();
        return await manager.AddAsync(entity);
    }

    public async Task<ActionResult<TEntity?>> UpdateAsync(Guid id, TUpdate form)
    {
        var entity = form.MapTo<TUpdate, TEntity>();
        return await manager.UpdateAsync(id, entity);
    }

    public async Task<ActionResult<PageList<TItem>>> FilterAsync<TItem>(TFilter filter)
    {
        return await manager.FilterAsync<TItem, TFilter>(filter);
    }

    public async Task<ActionResult<TEntity?>> GetDetailAsync(Guid id)
    {
        return await manager.FindAsync<TEntity>(id);
    }

}
