
using Share.Models.AuthDtos;

namespace Http.API.Controllers;

/// <summary>
/// 系统用户授权登录
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    //private readonly UserDataStore _store;
    private readonly IConfiguration _config;
    private readonly RedisService _redis;
    public AuthController(
        //UserDataStore userDataStore, 
        IConfiguration config, RedisService redis)
    {
        //_store = userDataStore;
        _config = config;
        _redis = redis;
    }

    /// <summary>
    /// 登录获取Token
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<AuthResult>> LoginAsync(LoginDto dto)
    {
        var user = new User();
        // TODO:查询用户
        //var user = await _store.Db.Where(u => u.UserName.Equals(dto.UserName))
        //    .Include(u => u.Roles)
        //    .FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("不存在该用户");
        }

        if (HashCrypto.Validate(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            var sign = _config.GetSection("Jwt")["Sign"];
            var issuer = _config.GetSection("Jwt")["Issuer"];
            var audience = _config.GetSection("Jwt")["Audience"];

            var role = user.Roles?.FirstOrDefault();
            //var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //1天后过期
            var jwt = new JwtService(sign, audience, issuer)
            {
                TokenExpires = 60 * 24 * 7,
            };
            var token = jwt.GetToken(user.Id.ToString(), role?.Name ?? "");
            // 登录状态存储到Redis
            //await _redis.SetValueAsync("login" + user.Id.ToString(), true, 60 * 24 * 7);

            return new AuthResult
            {
                Id = user.Id,
                Role = role?.Name ?? "",
                Token = token,
                Username = user.UserName
            };
        }
        else
        {
            return Problem("用户名或密码错误", title: "");
        }
    }

    /// <summary>
    /// 退出 
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<bool>> LogoutAsync([FromRoute] Guid id)
    {
        //var user = await _store.FindAsync(id);
        //if (user == null) return NotFound();
        // 清除redis登录状态
        //await _redis.Cache.RemoveAsync(id.ToString());
        return await Task.FromResult(true);
    }
}
