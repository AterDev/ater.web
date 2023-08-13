namespace Ater.Web.Abstraction;
/// <summary>
/// 应用组件配置
/// </summary>
public class AppComponentConfig
{
    /// <summary>
    /// 数据库支持:pgsql/sqlserver
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// 缓存支持:redis/memory/none
    /// </summary>
    public string? Cache { get; set; }

    /// <summary>
    /// 日志支持:serilog/otlp/none
    /// </summary>
    public string? Logging { get; set; }

    public bool Swagger { get; set; } = true;
    public bool Jwt { get; set; } = true;
}
