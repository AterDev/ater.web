using Http.API;
using Http.API.Worker;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
//{
//    if (eventArgs.Exception is OutOfMemoryException)
//    {
//        Console.WriteLine($"=== OutOfMemory: {eventArgs.Exception.Message}, {eventArgs.Exception.StackTrace}");
//    }
//    else
//    {
//        Console.WriteLine($"Caught exception: {eventArgs.Exception.Message}");
//    }
//};

// 1 添加默认组件
builder.AddDefaultComponents();
// 2 注册和配置Web服务依赖
builder.AddDefaultWebServices();
// 3 其他自定义选项及服务
builder.Services.AddSingleton<IEmailService, EmailService>();
WebApplication app = builder.Build();

// 使用中间件
app.UseDefaultWebServices();

using (app)
{
    // 初始化工作
    await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
    {
        IServiceProvider provider = scope.ServiceProvider;
        await InitDataTask.InitDataAsync(provider);
    }
    app.Run();
}

public partial class Program { }