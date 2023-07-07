namespace Core;
/// <summary>
/// Angular page 生成标记
/// </summary>
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

/// <summary>
/// 模块标记
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// 模块名称，区分大小写
    /// </summary>
    public string Name { get; init; }

    public ModuleAttribute(string name)
    {
        Name = name;
    }
}