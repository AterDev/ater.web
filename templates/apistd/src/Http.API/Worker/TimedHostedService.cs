﻿namespace Http.API.Worker;
/// <summary>
/// 后台计时器示例
/// </summary>
public class TimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TimedHostedService> _logger;
    public IServiceProvider Services { get; }
    private Timer? _timer = null;
    public TimedHostedService(
        IServiceProvider services,
        ILogger<TimedHostedService> logger
        )
    {
        Services = services;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _timer = new Timer(RunAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(60 * 2));
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError("{message},{source},{stackTrace}", ex.Message, ex.Source, ex.StackTrace);
            return Task.CompletedTask;
        }
    }


    /// <summary>
    /// 控制所有计时任务
    /// </summary>
    /// <param name="state"></param>
    public async void RunAsync(object? state)
    {
        try
        {
            await OperateDataBaseAsync(state);
        }
        catch (Exception ex)
        {
            _logger.LogError("{message},{source},{stackTrace}", ex.Message, ex.Source, ex.StackTrace);
        }
    }

    /// <summary>
    /// 数据库操作示例
    /// </summary>
    public async Task OperateDataBaseAsync(object? state)
    {
        using IServiceScope scope = Services.CreateScope();
        ISystemUserManager userManager = scope.ServiceProvider.GetRequiredService<ISystemUserManager>();
        List<SystemUser> recentPost = await userManager.Query.Db.OrderByDescending(x => x.CreatedTime)
            .Take(20)
            .ToListAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");
        _ = (_timer?.Change(Timeout.Infinite, 0));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
