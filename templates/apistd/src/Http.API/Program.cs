using System.Text.Encodings.Web;
using System.Text.Unicode;

using Ater.Web.Abstraction;

using Http.API;
using Http.API.Worker;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

// 1 基础组件
services.AddAppComponents(configuration);
services.AddWebComponents(configuration);

// 2 授权配置
services.AddAuthorizationBuilder()
    .AddPolicy(AppConst.User, policy => policy.RequireRole(AppConst.User))
    .AddPolicy(AppConst.AdminUser, policy => policy.RequireRole(AppConst.SuperAdmin, AppConst.AdminUser))
    .AddPolicy(AppConst.SuperAdmin, policy => policy.RequireRole(AppConst.SuperAdmin));

services.AddHttpContextAccessor();
services.AddTransient<IUserContext, UserContext>();
services.AddTransient<ITenantProvider, TenantProvider>();
// 3 数据及业务接口注入
services.AddManager();
// 其他模块Manager
//services.AddSystemModManagers();

// 4 其他自定义选项及服务
services.AddSingleton(typeof(CacheService));
services.AddSingleton<IEmailService, EmailService>();

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

app.MapDefaultEndpoints();
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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
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
