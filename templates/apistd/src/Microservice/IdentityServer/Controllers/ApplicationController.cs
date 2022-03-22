using OpenIddictApplication = IdentityServer.Models.OpenIddictApplication;

namespace IdentityServer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _manager;
    public ApplicationController(IOpenIddictApplicationManager manager)
    {
        _manager = manager;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OpenIddictApplication>> FindAsync([FromRoute] Guid id)
    {
        var application = await _manager.FindByIdAsync(id.ToString());
        if (application == null) return NotFound();
        return (ActionResult<OpenIddictApplication>)application;
    }

    [HttpGet]
    public List<OpenIddictApplication> List(int pageIndex = 1, int pageSize = 100)
    {
        if (pageIndex < 1) pageIndex = 1;
        var offset = (pageIndex - 1) * pageSize;
        return (List<OpenIddictApplication>)_manager.ListAsync(pageSize, offset);
    }

    [HttpPost]
    public async Task<ActionResult<OpenIddictApplication>> AddApplicationAsync(OpenIddictApplication application)
    {
        if (await _manager.FindByClientIdAsync(application.ClientId!) == null)
        {
            return (OpenIddictApplication)(await _manager.CreateAsync(application));
        }
        else
        {
            return Conflict();
        }
    }

    [HttpPut]
    public async Task<ActionResult<OpenIddictApplication>> EditApplicationAsync([FromRoute] Guid id, OpenIddictApplication update)
    {
        var application = await _manager.FindByIdAsync(id.ToString());
        if (application == null) return NotFound();
        await _manager.UpdateAsync(application, update);
        return update;
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteApplicationAsync([FromRoute] Guid id)
    {
        var application = await _manager.FindByIdAsync(id.ToString());
        if (application == null) return NotFound();
        if (application != null)
            await _manager.DeleteAsync(application);
        return Ok();
    }

}
