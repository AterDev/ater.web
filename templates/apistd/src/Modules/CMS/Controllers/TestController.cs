using Core.Entities.CmsEntities;

namespace CMS.Controllers;
/// <summary>
/// 模块接口
/// </summary>
[ApiExplorerSettings(GroupName = "client")]
public class TestController : RestControllerBase
{
    private readonly IBlogManager _blogManager;
    public TestController(IBlogManager blogManager)
    {
        _blogManager = blogManager;
    }

    [HttpGet]
    public async Task<List<Blog>> GetAsync()
    {

        var res = await _blogManager.Query.Db.ToListAsync();
        return res;
    }

}
