using Share.Models.UserDtos;
namespace Http.API.Controllers.AdminControllers;

/// <summary>
/// 用户账户
/// </summary>
/// <see cref="Application.Manager.UserManager"/>
[Authorize(AterConst.AdminUser)]
public class UserController(
    IUserContext user,
    ILogger<UserController> logger,
    UserManager manager
        ) : RestControllerBase<UserManager>(manager, user, logger)
{

    /// <summary>
    /// 筛选 ✅
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("filter")]
    public async Task<ActionResult<PageList<UserItemDto>>> FilterAsync(UserFilterDto filter)
    {
        return await _manager.FilterAsync(filter);
    }

    /// <summary>
    /// 新增 ✅
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<User>> AddAsync(UserAddDto dto)
    {
        // 判断重复用户名
        if (_manager.Query.Db.Any(q => q.UserName.Equals(dto.UserName)))
        {
            return Conflict(ErrorMsg.ExistUser);
        }
        User entity = await _manager.CreateNewEntityAsync(dto);
        return await _manager.AddAsync(entity);
    }

    /// <summary>
    /// 更新 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<User?>> UpdateAsync([FromRoute] Guid id, UserUpdateDto dto)
    {
        User? current = await _manager.GetCurrentAsync(id);
        if (current == null)
        {
            return NotFound(ErrorMsg.NotFoundResource);
        };
        return await _manager.UpdateAsync(current, dto);
    }

    /// <summary>
    /// 详情 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetDetailAsync([FromRoute] Guid id)
    {
        User? res = await _manager.FindAsync(id);
        return (res == null) ? NotFound() : res;
    }

    /// <summary>
    /// ⚠删除 ✅
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<User?>> DeleteAsync([FromRoute] Guid id)
    {
        // 注意删除权限
        User? entity = await _manager.GetCurrentAsync(id);
        if (entity == null)
        {
            return NotFound(ErrorMsg.NotFoundUser);
        };
        return await _manager.DeleteAsync(entity);
    }
}