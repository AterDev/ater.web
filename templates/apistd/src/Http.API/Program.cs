using System.Text.Encodings.Web;
using System.Text.Unicode;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

// 1 基础组件
services.AddAppComponents(configuration);
services.AddWebComponents(configuration);

// 2 授权配置
services.AddAuthorization(options =>
{
    options.AddPolicy(AppConst.User, policy =>
        policy.RequireRole(AppConst.AdminUser, AppConst.User));
    options.AddPolicy(AppConst.AdminUser, policy =>
        policy.RequireRole(AppConst.AdminUser));
});

// 3 数据及业务接口注入
services.AddHttpContextAccessor();
services.AddTransient<IUserContext, UserContext>();
services.AddDataStore();
services.AddManager();

// 4 其他自定义选项及服务
services.AddSingleton(typeof(CacheService));

services.AddControllers()
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = context =>
        {
            return new CustomBadRequest(context, null);
        };
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors("default");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/client/swagger.json", name: "client");
        c.SwaggerEndpoint("/swagger/admin/swagger.json", "admin");
    });
}
else
{
    app.UseCors("default");
    //app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

// 异常统一处理
app.UseExceptionHandler(ExceptionHandler.Handler());

app.UseHealthChecks("/health");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.MapFallbackToFile("index.html");

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

// for test project
public partial class Program { }
