namespace Core;
[AttributeUsage(AttributeTargets.Class)]
public class NgPageAttribute : Attribute
{
    /// <summary>
    /// 所属模块
    /// </summary>
    public string Module { get; init; }
    /// <summary>
    /// 路由,会默认添加模块名作为前缀
    /// </summary>
    public string Route { get; init; }

    public NgPageAttribute(string module, string route)
    {
        Module = module;
        Route = route;
    }
}
