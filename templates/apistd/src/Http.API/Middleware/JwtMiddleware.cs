using System.Security.Claims;
using System.Text.Json;
using Ater.Web.Abstraction;

namespace Http.API.Middleware;

/// <summary>
/// 在进入验证前，对token进行额外验证
/// </summary>
public class JwtMiddleware(RequestDelegate next, CacheService redis, ILogger<JwtMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly CacheService _cache = redis;
    private readonly ILogger<JwtMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        // 可匿名访问的放行
        var endpoint = context.GetEndpoint();
        var allowAnon = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        if (allowAnon)
        {
            await _next(context);
            return;
        }

        string token = context.Request.Headers[AppConst.Authorization].FirstOrDefault()?.Split(" ").Last() ?? string.Empty;
        string client = context.Request.Headers[AppConst.ClientHeader].FirstOrDefault() ?? string.Empty;

        if (token == null)
        {
            await SetResponseAndComplete(context, 401);
            return;
        }
        try
        {
            var id = JwtService.GetClaimValue(token, ClaimTypes.NameIdentifier);
            // TODO:策略判断
            if (id.NotEmpty())
            {
                var key = AppConst.LoginCachePrefix + client + id;
                var exist = _cache.GetValue<bool>(key);

                if (!exist)
                {
                    await SetResponseAndComplete(context, 401);
                    return;
                }
                else
                {
                    // 已经存在，则顶替
                }

                _cache.Cache.Refresh(key);
                await _next(context);
                return;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }
    }
    private async Task SetResponseAndComplete(HttpContext context, int statusCode, string? msg = "无效的凭证")
    {
        var res = new
        {
            Title = msg,
            Detail = msg,
            Status = statusCode,
            TraceId = context.TraceIdentifier
        };

        string content = JsonSerializer.Serialize(res);
        byte[] byteArray = Encoding.UTF8.GetBytes(content);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.Body.WriteAsync(byteArray, 0, byteArray.Length);
        await context.Response.CompleteAsync();
    }
}