using Http.API.Infrastructure;
namespace Http.API.Controllers;


/// <summary>
/// 统一使用的接口
/// </summary>
public class CommonController : RestControllerBase
{
    private readonly IWebHostEnvironment _environment;


    public CommonController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="upload"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [RequestSizeLimit(1024 * 1024 * 1)]
    public async Task<ActionResult<UploadResult>> UploadImgAsync(IFormFile upload)
    {
        if (upload == null)
        {
            return Problem("没有上传的文件", title: "业务错误");
        }
        //获取静态资源文件根目录
        string webRootPath = _environment.WebRootPath;
        string dir = Path.Combine(webRootPath, "Uploads", "images");
        if (upload.Length > 0)
        {
            if (!Directory.Exists(dir))
            {
                _ = Directory.CreateDirectory(dir);
            }
            string fileExt = Path.GetExtension(upload.FileName).ToLowerInvariant();
            long fileSize = upload.Length; //获得文件大小，以字节为单位
            //判断后缀是否是图片
            string[] permittedExtensions = new string[] { ".jpeg", ".jpg", ".png", ".bmp", ".svg", ".webp" };

            if (fileExt == null)
            {
                return Problem("上传的文件没有后缀");
            }
            if (!permittedExtensions.Contains(fileExt))
            {
                return Problem("不支持的图片格式");
            }
            if (fileSize > 1024 * 1024 * 1) //M
            {
                //上传的文件不能大于1M
                return Problem("上传的图片应小于1M");
            }

            string fileName = HashCrypto.Md5FileHash(upload.OpenReadStream());
            var blobPath = Path.Combine("movement", DateTime.UtcNow.ToString("yyyy-MM-dd"), fileName + fileExt);

            // TODO:上传云存储

            // TODO:返回上传后的路径对象
            return new UploadResult()
            {
                FilePath = "",
                Url = "",
            };
        }
        return Problem("文件不正确", title: "业务错误");
    }
}
