using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Share.Options;

namespace Application.Services;
/// <summary>
/// 存储服务
/// </summary>
public class StorageService
{
    private readonly AzureOption option;
    private static readonly string BlobName = "blogs";

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
    public async Task<string> UploadAsync(Stream stream, string fileName)
    {
        if (option.BlobConnectionString == null)
        {
            _logger.LogError("BlobConnectionString is null");
            return string.Empty;
        }

        BlobContainerClient container = new(option.BlobConnectionString, BlobName);
        BlobClient blob = container.GetBlobClient(fileName);
        try
        {

            Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo> res = await blob.UploadAsync(stream, false);
            return blob.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            if (ex is Azure.RequestFailedException exception)
            {
                if (exception.Status == 409)
                {
                    _logger.LogInformation(exception.ErrorCode);
                    return blob.Uri.AbsoluteUri;
                }
                return string.Empty;
            }
            else
            {
                _logger.LogError("up to blob error:{message},{starckTrace}", ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }
    }
}
