using Core.Entities.System;
using Http.API.Infrastructure;
using Share.Models.SystemUserDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 系统用户
/// </summary>
public class SystemUserController : RestControllerBase<ISystemUserManager>
{
    private readonly IWebHostEnvironment _environment;
    public SystemUserController(
        IUserContext user,
        ILogger<SystemUserController> logger,
        ISystemUserManager manager
,
        IWebHostEnvironment environment) : base(manager, user, logger)
    {
        _environment = environment;
    }

    /// <summary>
    /// 筛选
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<SystemUserItemDto>>> FilterAsync(SystemUserFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<SystemUser>> AddAsync(SystemUserAddDto form)
    {
        SystemUser entity = form.MapTo<SystemUserAddDto, SystemUser>();
        return await manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<SystemUser?>> UpdateAsync([FromRoute] Guid id, SystemUserUpdateDto form)
    {
        SystemUser? current = await manager.GetCurrentAsync(id);
        return current == null ? (ActionResult<SystemUser?>)NotFound() : (ActionResult<SystemUser?>)await manager.UpdateAsync(current, form);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemUser?>> GetDetailAsync([FromRoute] Guid id)
    {
        SystemUser? res = await manager.FindAsync(id);
        return res == null ? NotFound() : res;
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    public async Task<ActionResult<UploadResult>> UploadImgAsync(IFormFile file)
    {
        if (file == null)
        {
            return Problem("没有上传的文件", title: "业务错误");
        }
        //获取静态资源文件根目录
        string webRootPath = _environment.WebRootPath;
        string dir = Path.Combine(webRootPath, "Uploads", "images");
        if (file.Length > 0)
        {
            if (!Directory.Exists(dir))
            {
                _ = Directory.CreateDirectory(dir);
            }
            string fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
            long fileSize = file.Length; //获得文件大小，以字节为单位
            //判断后缀是否是图片
            string[] permittedExtensions = new string[] { ".jpeg", ".jpg", ".png" };

            if (fileExt == null)
            {
                return Problem("上传的文件没有后缀");
            }
            if (!permittedExtensions.Contains(fileExt))
            {
                return Problem("请上传jpg、png格式的图片");
            }
            if (fileSize > 1024 * 1024 * 1) //M
            {
                //上传的文件不能大于1M
                return Problem("上传的图片应小于1M");
            }
            using MemoryStream stream = new();
            await file.CopyToAsync(stream);

            string fileName = HashCrypto.Md5FileHash(stream);
            string filePath = Path.Combine(dir, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                // 写入文件
                using FileStream fileStream = System.IO.File.Create(filePath);
                file.CopyTo(fileStream);
                fileStream.Close();
            }
            string host = Request.Scheme + "://" + Request.Host.Value;
            string path = Path.Combine(host, "Uploads", "images", fileName + fileExt);
            return new UploadResult()
            {
                FilePath = path,
                Url = path,
            };
        }
        return Problem("文件不正确", title: "业务错误");
    }

    /// <summary>
    /// ⚠删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<SystemUser?>> DeleteAsync([FromRoute] Guid id)
    {
        SystemUser? entity = await manager.GetCurrentAsync(id);
        return entity == null ? (ActionResult<SystemUser?>)NotFound() : (ActionResult<SystemUser?>)await manager.DeleteAsync(entity);
    }
}