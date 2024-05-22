using Application;
using Entity.FileManagerMod;
using FileManagerMod.Models.FileDataDtos;

using Microsoft.AspNetCore.Http;

namespace FileManagerMod.Controllers.AdminControllers;

/// <summary>
/// 文件数据
/// </summary>
/// <see cref="FileManagerMod.Manager.FileDataManager"/>
public class FileDataController(
    IUserContext user,
    ILogger<FileDataController> logger,
    FileDataManager manager,
    FolderManager folderManager) : RestControllerBase<FileDataManager>(manager, user, logger)
{
    private readonly FolderManager _folderManager = folderManager;

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<FileDataItemDto>>> FilterAsync(FileDataFilterDto filter)
    {
        return await manager.FilterAsync(filter);
    }

    /// <summary>
    /// 上传 ✅
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> AddAsync(Guid? folderId, List<IFormFile> files)
    {
        Folder? folder = null;
        if (folderId != null)
        {
            folder = await _folderManager.GetCurrentAsync(folderId.Value);
            if (folder == null)
            {
                return NotFound("错误的目录");
            }
        }
        List<FileData> data = await manager.CreateNewEntityAsync(files, folder);
        return await manager.AddFilesAsync(data);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [RequestSizeLimit(1024 * 500)]
    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadAsync(string path, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        FileData fileData = await manager.AddFileAsync(file.OpenReadStream(), path, file.FileName);
        return fileData != null ? fileData.Md5 : Problem("上传失败");
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<FileData?>> GetDetailAsync([FromRoute] Guid id)
    {
        FileData? res = await manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<FileData?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        FileData? entity = await manager.GetCurrentAsync(id);
        if (entity == null)
        {
            return NotFound();
        };
        return await manager.DeleteAsync(entity, false);
    }
}