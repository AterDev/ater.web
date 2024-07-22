using Ater.Web.Abstraction.Interface;
using EntityFramework.DBProvider;
using Microsoft.Extensions.Hosting;

namespace SystemMod.Worker;
/// <summary>
/// 日志记录任务
/// </summary>
public class SystemLogTaskHostedService(IServiceProvider serviceProvider, IEntityTaskQueue<SystemLogs> queue, ILogger<SystemLogTaskHostedService> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<SystemLogTaskHostedService> _logger = logger;
    private readonly IEntityTaskQueue<SystemLogs> _taskQueue = queue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"🚀 System Log Hosted Service is running.");
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
                context.Add(log);
                await context.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("✍️ New Log {name} is saved.", log.TargetName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {name}.", nameof(log.TargetName));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🛑 System Log Hosted Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
