﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Ater.Web.Core.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Ater.Web.Extension.Middleware;

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
        Endpoint? endpoint = context.GetEndpoint();
        var allowAnon = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        var token = context.Request.Headers[AterConst.Authorization].FirstOrDefault()?.Split(" ").Last() ?? string.Empty;
        var client = context.Request.Headers[AterConst.ClientHeader].FirstOrDefault() ?? AterConst.Web;
        if (allowAnon || token.IsEmpty())
        {
            await _next(context);
            return;
        }
        // 判断 token 是否有效
        JwtSecurityTokenHandler tokenHandler = new();
        if (tokenHandler.CanReadToken(token) == false)
        {
            await _next(context);
        }
        var id = JwtService.GetClaimValue(token, ClaimTypes.NameIdentifier);
        // 策略判断
        if (id.NotEmpty())
        {
            var securityPolicyStr = _cache.GetValue<string>(AterConst.LoginSecurityPolicy);
            var securityPolicy = JsonSerializer.Deserialize<LoginSecurityPolicy>(securityPolicyStr!);

            if (securityPolicy == null || !securityPolicy.IsEnable)
            {
                await _next(context);
                return;
            }
            if (securityPolicy.SessionLevel == SessionLevel.OnlyOne)
            {
                client = AterConst.AllPlatform;
            }
            var key = AterConst.LoginCachePrefix + client + id;
            var cacheToken = _cache.GetValue<string>(key);
            if (cacheToken.IsEmpty())
            {
                await SetResponseAndComplete(context, 401);
                return;
            }

            if (securityPolicy.SessionLevel != SessionLevel.None && cacheToken != token)
            {
                await SetResponseAndComplete(context, 401, "账号已在其他客户端登录");
                return;
            }

            _cache.Cache.Refresh(key);
            await _next(context);
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

        var content = JsonSerializer.Serialize(res);
        byte[] byteArray = Encoding.UTF8.GetBytes(content);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.Body.WriteAsync(byteArray);
        await context.Response.CompleteAsync();
    }
}