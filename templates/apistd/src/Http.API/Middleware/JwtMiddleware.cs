using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Http.API.Middleware;

/// <summary>
/// 在进入验证前，对token进行额外验证
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CacheService _cache;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, CacheService redis, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _cache = redis;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var allowAnon = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        if (allowAnon)
        {
            await _next(context);
            return;
        }
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.CompleteAsync();
            return;
        }

        var res = new
        {
            Title = "您已经被挤下线或登录已过期",
            Detail = "您已经被挤下线或登录已过期",
            Status = 401,
            TraceId = context.TraceIdentifier
        };

        string content = JsonSerializer.Serialize(res);
        byte[] byteArray = Encoding.UTF8.GetBytes(content);

        context.Response.ContentType = "application/json";
        // 获取Response.Body的输出流
        Stream responseStream = context.Response.Body;

        try
        {
            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);
            string id = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            long exp = long.Parse(jwtToken.Claims.First(c => c.Type == "exp").Value);
            if (!string.IsNullOrWhiteSpace(id))
            {
                string? oldToken = _cache.GetValue<string>($"token_{id}");
                if (oldToken is not null)
                {
                    //比较相同用户的token过期时间
                    JwtSecurityToken oldJwtToken = tokenHandler.ReadJwtToken(oldToken);
                    long oldexp = long.Parse(oldJwtToken.Claims.First(c => c.Type == "exp").Value);
                    if (exp < oldexp)
                    {
                        context.Response.StatusCode = 401;

                        // 将数据写入Response.Body
                        responseStream.Write(byteArray, 0, byteArray.Length);

                        // 必须手动刷新Response.Body，以确保数据被发送到客户端
                        responseStream.Flush();
                        await context.Response.CompleteAsync();
                        return;
                    }
                    if (!oldToken!.Equals(token))
                    {
                        await _cache.RemoveAsync($"token_{id}");
                    }
                    await _cache.SetValueAsync($"token_{id}", token, 30 * 60);
                    await _next(context);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
        context.Response.StatusCode = 401;
        // 将数据写入Response.Body
        responseStream.Write(byteArray, 0, byteArray.Length);

        // 必须手动刷新Response.Body，以确保数据被发送到客户端
        responseStream.Flush();
        await context.Response.CompleteAsync();
    }
}