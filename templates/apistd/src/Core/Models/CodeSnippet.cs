namespace Core.Models;

/// <summary>
/// 代码片段
/// </summary>
public class CodeSnippet : EntityBase
{
    /// <summary>
    /// 实体名称
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    /// <summary>
    /// 实体定义内容
    /// </summary>
    [MaxLength(4000)]
    public string? Content { get; set; }
    /// <summary>
    /// 所属类库
    /// </summary>
    public Library? Library { get; set; }
    /// <summary>
    /// 语言类型
    /// </summary>
    public Language Language { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public CodeType CodeType { get; set; }

}

/// <summary>
/// 代码片段类型
/// </summary>
public enum CodeType
{
    /// <summary>
    /// 函数
    /// </summary>
    Function,
    /// <summary>
    /// 工具
    /// </summary>
    Util,
    /// <summary>
    /// 服务
    /// </summary>
    Service,
    /// <summary>
    /// 控制器
    /// </summary>
    Controller,
    /// <summary>
    /// 实体
    /// </summary>
    Entity,
    /// <summary>
    /// 模型
    /// </summary>
    Model,
    /// <summary>
    /// 算法
    /// </summary>
    Algorithm,
    /// <summary>
    /// 配置
    /// </summary>
    Config,
    /// <summary>
    /// 接口
    /// </summary>
    Interface,
    Else
}

/// <summary>
/// 编程语言
/// </summary>
public enum Language
{
    Csharp,
    Fsharp,
    Java,
    Php,
    Python,
    Kotlin,
    Swift,
    Typescript,
    Javascript,
    Html,
    Css,
    Dart,
    Rust,
    Cpp,
    Golang,
    Node,
    Deno,
    Markdown,
    Text,
    Shell,
    Powershell,
    Json,
    Xml,
    Else
}
