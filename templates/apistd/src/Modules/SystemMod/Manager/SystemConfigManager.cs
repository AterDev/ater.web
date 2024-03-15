using System.Text.Json;
using Ater.Web.Extension.Models;
using Ater.Web.Extension.Services;
using SystemMod.Models.SystemConfigDtos;

namespace SystemMod.Manager;
/// <summary>
/// 系统配置
/// </summary>
public class SystemConfigManager(
    DataAccessContext<SystemConfig> dataContext,
    ILogger<SystemConfigManager> logger,
    IConfiguration configuration,
    CacheService cache) : ManagerBase<SystemConfig, SystemConfigUpdateDto, SystemConfigFilterDto, SystemConfigItemDto>(dataContext, logger)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly CacheService _cache = cache;

    /// <summary>
    /// 创建待添加实体
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<SystemConfig> CreateNewEntityAsync(SystemConfigAddDto dto)
    {
        SystemConfig entity = dto.MapTo<SystemConfigAddDto, SystemConfig>();
        // other required props
        return await Task.FromResult(entity);
    }

    public override async Task<SystemConfig> UpdateAsync(SystemConfig entity, SystemConfigUpdateDto dto)
    {
        return await base.UpdateAsync(entity, dto);
    }

    public override async Task<PageList<SystemConfigItemDto>> FilterAsync(SystemConfigFilterDto filter)
    {
        Queryable = Queryable
            .WhereNotNull(filter.Key, q => q.Key == filter.Key)
            .WhereNotNull(filter.GroupName, q => q.GroupName == filter.GroupName);

        return await Query.FilterAsync<SystemConfigItemDto>(Queryable, filter.PageIndex, filter.PageSize, filter.OrderBy);
    }

    /// <summary>
    /// 获取枚举信息
    /// </summary>
    /// <returns></returns>
    public async Task<Dictionary<string, List<EnumDictionary>>> GetEnumConfigsAsync()
    {
        // TODO:程序启动时更新缓存
        Dictionary<string, List<EnumDictionary>>? res = _cache.GetValue<Dictionary<string, List<EnumDictionary>>>(AppConst.EnumCacheName);
        if (res == null || res.Count == 0)
        {
            Dictionary<string, List<EnumDictionary>> data = EnumHelper.GetAllEnumInfo();
            await _cache.SetValueAsync(AppConst.EnumCacheName, data, null);
            return data;
        }
        return res;
    }

    /// <summary>
    /// 当前用户所拥有的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SystemConfig?> GetOwnedAsync(Guid id)
    {
        IQueryable<SystemConfig> query = Command.Db.Where(q => q.Id == id);
        // 获取用户所属的对象
        // query = query.Where(q => q.User.Id == _userContext.UserId);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// 获取登录安全策略
    /// </summary>
    /// <returns></returns>
    public LoginSecurityPolicy GetLoginSecurityPolicy()
    {
        // 优先级：数据库>配置文件>默认配置
        var policy = new LoginSecurityPolicy();

        var configString = Query.Db.Where(q => q.GroupName.Equals(AppConst.SystemGroup) && q.Key.Equals(AppConst.LoginSecurityPolicy))
            .Select(q => q.Value)
            .FirstOrDefault();

        if (configString != null)
        {
            policy = JsonSerializer.Deserialize<LoginSecurityPolicy>(configString);
        }
        else
        {
            var config = _configuration.GetSection(AppConst.LoginSecurityPolicy);
            if (config.Exists())
            {
                policy = config.Get<LoginSecurityPolicy>();
            }
        }
        return policy ?? new LoginSecurityPolicy();
    }
}
