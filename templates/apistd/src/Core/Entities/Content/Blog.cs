namespace Core.Entities.Content;
/// <summary>
/// 博客
/// </summary>
public class Blog : TextBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(200)]
    public string? TranslateTitle { get; set; }
    /// <summary>
    /// 翻译内容
    /// </summary>
    [MaxLength(12000)]
    public string? TranslateContent { get; set; }
    /// <summary>
    /// 语言类型
    /// </summary>
    public LanguageType LanguageType { get; set; } = LanguageType.CN;

    /// <summary>
    /// 全站类别
    /// </summary>
    public BlogType BlogType { get; set; }

    /// <summary>
    /// 是否审核
    /// </summary>
    public bool IsAudit { get; set; } = false;
    /// <summary>
    /// 是否公开
    /// </summary>
    public bool IsPublic { get; set; } = true;
    /// <summary>
    /// 是否原创
    /// </summary>
    public bool IsOriginal { get; set; }
    public required User User { get; set; }
    /// <summary>
    /// 所属目录
    /// </summary>
    public required Catalog Catalog { get; set; }
    public List<Tags>? Tags { get; set; }
    /// <summary>
    /// 浏览量
    /// </summary>
    public int ViewCount { get; set; } = 0;
}

public enum BlogType
{
    /// <summary>
    /// 资讯
    /// </summary>
    [Description("资讯")]
    News,
    /// <summary>
    /// 开源
    /// </summary>
    [Description("开源和工具")]
    OpenSource,
    /// <summary>
    /// 语言及框架
    /// </summary>
    [Description("语言及框架")]
    LanguageAndFramework,
    /// <summary>
    /// 数据和AI
    /// </summary>
    [Description("AI和数据")]
    DataAndAI,
    /// <summary>
    /// DevOps
    /// </summary>
    [Description("云与DevOps")]
    CloudAndDevOps,

    /// <summary>
    /// 见解与分析
    /// </summary>
    [Description("见解与分析")]
    View,
    /// <summary>
    /// 其它
    /// </summary>
    [Description("其它")]
    Else
}

public enum LanguageType
{
    /// <summary>
    /// 中文
    /// </summary>
    [Description("中文")]
    CN,
    /// <summary>
    /// 英文
    /// </summary>
    [Description("英文")]
    EN
}
