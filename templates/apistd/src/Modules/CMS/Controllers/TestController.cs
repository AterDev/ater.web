namespace CMS.Controllers;
/// <summary>
/// 模块接口
/// </summary>
[ApiExplorerSettings(GroupName = "client")]
public class TestController : RestControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "hello world";
    }

}
