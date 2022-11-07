using System.Threading.Channels;

namespace Application.Services;


public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundWorkItemAsync(Func<object, ValueTask> workItem);

    ValueTask<Func<object, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);
}
/// <summary>
/// 后台队列定义
/// </summary>
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<object, ValueTask>> _queue;

    public BackgroundTaskQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<object, ValueTask>>(options);
    }

    public async ValueTask QueueBackgroundWorkItemAsync(Func<object, ValueTask> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<object, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);
        return workItem;
    }
}
