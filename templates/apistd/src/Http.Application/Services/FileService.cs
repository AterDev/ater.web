namespace Http.Application.Services;

/// <summary>
/// 文件服务
/// </summary>
public class FileService
{
    public string LocalPath { get; }

    private readonly IHostEnvironment _env;
    public FileService(
        IHostEnvironment env)
    {
        _env = env;
        LocalPath = Path.Combine(_env.ContentRootPath, "wwwroot", "Uploads");
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public string SaveFile(string path, Stream stream)
    {
        var filePath = Path.Combine(LocalPath, path);
        if (File.Exists(filePath))
        {
            return filePath;
        }
        // 创建目录
        var dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath!);
        }
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
        return filePath;
    }
}
