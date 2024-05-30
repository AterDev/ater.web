using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Http.API.Middleware;

/// <summary>
/// swagger �����м��
/// </summary>
public class CachedSwaggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public CachedSwaggerMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger/admin/swagger.json"))
        {
            var swaggerJson = _cache.Get<string>("swagger_admin");
            if (swaggerJson == null)
            {
                var swaggerProvider = context.RequestServices.GetRequiredService<ISwaggerProvider>();
                var swaggerDoc = swaggerProvider.GetSwagger("admin");
                swaggerJson = swaggerDoc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
                _cache.Set("swagger_admin", swaggerJson);
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(swaggerJson);
            return;
        }
        if (context.Request.Path.StartsWithSegments("/swagger/client/swagger.json"))
        {
            var swaggerJson = _cache.Get<string>("swagger_client");
            if (swaggerJson == null)
            {
                var swaggerProvider = context.RequestServices.GetRequiredService<ISwaggerProvider>();
                var swaggerDoc = swaggerProvider.GetSwagger("client");
                swaggerJson = swaggerDoc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
                _cache.Set("swagger_client", swaggerJson);
            }
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(swaggerJson);
            return;
        }
        await _next(context);
    }
}

public static class CachedSwaggerMiddlewareExtensions
{
    /// <summary>
    /// ����swagger json���������������ʹ��
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCachedSwagger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CachedSwaggerMiddleware>();
    }
}
