using Microsoft.Extensions.Caching.Distributed;

namespace Http.Application.Services;

/// <summary>
/// 简单封装对象的存储和获取
/// </summary>
public class RedisService
{
    public IDistributedCache Cache => _cache;

    private readonly IDistributedCache _cache;
    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 保存到缓存
    /// </summary>
    /// <param name="data">值</param>
    /// <param name="key">键</param>
    /// <param name="minutes">过期时间</param>
    /// <returns></returns>
    public async Task SetValueAsync(string key, object data, int minutes)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(data);
        await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(minutes)
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
        var bytes = _cache.Get(key);
        if (bytes == null || bytes.Length < 1)
        {
            return default;
        }
        var readOnlySpan = new ReadOnlySpan<byte>(bytes);
        return JsonSerializer.Deserialize<T>(readOnlySpan);
    }
}
