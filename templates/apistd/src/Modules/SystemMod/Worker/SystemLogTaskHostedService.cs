using System.Threading.Channels;
using EntityFramework.DBProvider;
using Microsoft.Extensions.Hosting;

namespace SystemMod.Worker;
/// <summary>
/// 日志记录任务
/// </summary>
public class SystemLogTaskHostedService(IServiceProvider serviceProvider, SystemLogTaskQueue queue, ILogger<SystemLogTaskHostedService> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<SystemLogTaskHostedService> _logger = logger;
    private readonly SystemLogTaskQueue _taskQueue = queue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Log Hosted Service is running.");
        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CommandDbContext>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var log = await _taskQueue.DequeueAsync(stoppingToken);
            try
            {
                // 查询今天是否有相同的日志
                var today = DateTime.UtcNow.Date;
                var exist = await context.SystemLogs.AnyAsync(l => l.ActionType == log.ActionType
                    && l.ActionUserName == log.ActionUserName
                    && l.SystemUserId == log.SystemUserId
                    && l.CreatedTime >= today
                    && l.TargetName == log.TargetName, stoppingToken);

                if (!exist)
                {
                    context.Add(log);
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Log {name} is saved.", log.TargetName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {name}.", nameof(log.TargetName));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Log Hosted Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}

/// <summary>
/// 业务日志队列任务
/// </summary>
public class SystemLogTaskQueue
{
    private readonly Channel<SystemLogs> _queue;

    public SystemLogTaskQueue(int capacity = 1000)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<SystemLogs>(options);
    }

    public async ValueTask AddItemAsync(SystemLogs workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<SystemLogs> DequeueAsync(CancellationToken cancellationToken)
    {
        SystemLogs workItem = await _queue.Reader.ReadAsync(cancellationToken);
        return workItem;
    }

}