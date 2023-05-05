﻿using Microsoft.Extensions.Caching.Distributed;
namespace Application.Services;

/// <summary>
/// 简单封装对象的存储和获取
/// </summary>
public class CacheService
{
    private readonly IDistributedCache _cache;
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 缓存存储
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    public async Task SetValueAsync(string key, object data, int? expiration)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(data);
        var option = new DistributedCacheEntryOptions();
        if (expiration != null)
        {
            option.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiration.Value);
        }
        await _cache.SetAsync(key, bytes, option);
    }

    /// <summary>
    /// 保存到缓存
    /// </summary>
    /// <param name="data">值</param>
    /// <param name="key">键</param>
    /// <param name="sliding">相对过期时间</param>
    /// <param name="expiration">绝对过期时间</param>
    /// <returns></returns>
    public async Task SetValueAsync(string key, object data, int sliding, int expiration)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(data);
        await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromSeconds(sliding),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiration)
        });
    }
    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T? GetValue<T>(string key)
    {
        byte[]? bytes = _cache.Get(key);
        if (bytes == null || bytes.Length < 1)
        {
            return default;
        }
        ReadOnlySpan<byte> readOnlySpan = new(bytes);
        return JsonSerializer.Deserialize<T>(readOnlySpan);
    }
}
