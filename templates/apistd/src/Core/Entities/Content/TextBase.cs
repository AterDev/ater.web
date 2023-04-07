using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Content;
public class TextBase : EntityBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(100)]
    public required string Title { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(300)]
    public string? Description { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    [MaxLength(10000)]
    public required string Content { get; set; }
    /// <summary>
    /// 作者
    /// </summary>
    [MaxLength(200)]
    public required string Authors { get; set; }
}
