using Microsoft.Extensions.Caching.Distributed;
namespace Application.Services;

/// <summary>
/// 简单封装对象的存储和获取
/// </summary>
public class RedisService
{
    public IDistributedCache Cache { get; }

    public RedisService(IDistributedCache cache)
    {
        Cache = cache;
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
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(data);
        await Cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
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
        byte[]? bytes = Cache.Get(key);
        if (bytes == null || bytes.Length < 1)
        {
            return default;
        }
        ReadOnlySpan<byte> readOnlySpan = new(bytes);
        return JsonSerializer.Deserialize<T>(readOnlySpan);
    }
}
