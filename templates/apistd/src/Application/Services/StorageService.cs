using Microsoft.Extensions.Options;

using Share.Options;

namespace Application.Services;
/// <summary>
/// 存储服务
/// </summary>
public class StorageService
{
    private readonly AzureOption option;
    private readonly ILogger<StorageService> _logger;

    public StorageService(IOptions<AzureOption> option, ILogger<StorageService> logger)
    {
        _logger = logger;
        this.option = option.Value;
        if (string.IsNullOrWhiteSpace(this.option.BlobConnectionString))
        {
            _logger.LogError("BlobConnectionString is null");
        }

    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string Upload(Stream stream, string fileName)
    {
        throw new NotImplementedException();
    }
}
