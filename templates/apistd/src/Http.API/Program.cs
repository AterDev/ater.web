using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Application.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

#region logger
// config logger
var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
Action<ResourceBuilder> configureResource = r => r.AddService(
    "dusi.dev", serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName);
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.ConfigureResource(configureResource);
    options.AddConsoleExporter();
});

builder.Services.Configure<OpenTelemetryLoggerOptions>(opt =>
{
    opt.IncludeScopes = true;
    opt.ParseStateValues = true;
    opt.IncludeFormattedMessage = true;
});
#endregion



var services = builder.Services;
var configuration = builder.Configuration;
services.AddHttpContextAccessor();
// database sql
var connectionString = configuration.GetConnectionString("Default");
services.AddDbContextPool<QueryDbContext>(option =>
{
    option.UseNpgsql(connectionString, sql =>
    {
        sql.MigrationsAssembly("EntityFramework.Migrator");
        sql.CommandTimeout(10);
    });
});
services.AddDbContextPool<CommandDbContext>(option =>
{
    option.UseNpgsql(connectionString, sql =>
    {
        sql.MigrationsAssembly("EntityFramework.Migrator");
        sql.CommandTimeout(10);
    });
});

services.AddDataStore();
services.AddManager();

// redis
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("Redis");
//    options.InstanceName = builder.Configuration.GetConnectionString("RedisInstanceName");
//});
//services.AddSingleton(typeof(RedisService));


#region 接口相关内容:jwt/授权/cors
// use jwt
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.SaveToken = true;
    var sign = configuration.GetSection("Jwt")["Sign"];
    if (string.IsNullOrEmpty(sign))
    {
        throw new Exception("未找到有效的jwt配置");
    }
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sign)),
        ValidIssuer = configuration.GetSection("Jwt")["Issuer"],
        ValidAudience = configuration.GetSection("Jwt")["Audience"],
        ValidateIssuer = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true
    };
});

// 验证
services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy =>
        policy.RequireRole("Admin", "User"));
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});

// cors配置 
services.AddCors(options =>
{
    options.AddPolicy("default", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
#endregion

services.AddHealthChecks();
// api 接口文档设置
services.AddOpenApiDocument(c =>
{
    c.GenerateXmlObjects = true;
    c.GenerateEnumMappingDescription = true;
    c.UseControllerSummaryAsTagDescription = true;
    c.PostProcess = (document) =>
    {
        document.Info.Title = "MyProjectName";
        document.Info.Description = "Api 文档";
        document.Info.Version = "1.0";
    };
    c.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Bearer {your JWT token}."
    });
    c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
services.AddControllers()
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = context =>
        {
            return new CustomBadRequest(context, null);
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

var app = builder.Build();

// 初始化工作
await using (var scope = app.Services.CreateAsyncScope())
{
    var provider = scope.ServiceProvider;
    await InitDataTask.InitDataAsync(provider);
}

if (app.Environment.IsDevelopment())
{
    app.UseCors("default");
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi3(c => { c.DocumentTitle = "文档"; });
}
else
{
    // 生产环境需要新的配置
    app.UseCors("default");
    //app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
// 异常统一处理
//app.UseExceptionHandler(handler =>
//{
//    handler.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        var result = new
//        {
//            Title = "程序内部错误:" + exception?.Message,
//            Detail = exception?.Source,
//            Status = 500,
//            TraceId = context.TraceIdentifier
//        };
//        await context.Response.WriteAsJsonAsync(result);
//    });
//});

app.UseHealthChecks("/health");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }