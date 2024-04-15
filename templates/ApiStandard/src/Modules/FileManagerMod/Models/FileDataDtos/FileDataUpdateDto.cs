namespace FileManagerMod.Models.FileDataDtos;
/// <summary>
/// 文件数据更新时请求结构
/// </summary>
/// <see cref="Entity.FileManager.FileData"/>
public class FileDataUpdateDto
{
    /// <summary>
    /// 文件名
    /// </summary>
    [MaxLength(200)]
    public string? FileName { get; set; }
    /// <summary>
    /// 扩展名
    /// </summary>
    [MaxLength(20)]
    public string? Extension { get; set; }
    /// <summary>
    /// md5值
    /// </summary>
    [MaxLength(100)]
    public string? Md5 { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    [MaxLength(1024 * 1024 * 2)]
    public byte[]? Content { get; set; }

}
