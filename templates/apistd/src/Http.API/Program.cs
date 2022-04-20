using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
services.AddHttpContextAccessor();
// database sql
var connectionString = configuration.GetConnectionString("Default");
services.AddDbContextPool<ContextBase>(option =>
{
    option.UseNpgsql(connectionString, sql => { sql.MigrationsAssembly("EntityFramework.Migrator"); });
});

// redis
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("Redis");
//    options.InstanceName = builder.Configuration.GetConnectionString("RedisInstanceName");
//});
//services.AddSingleton(typeof(RedisService));

services.AddScoped(typeof(FileService));

#region �ӿ��������:jwt/��Ȩ/cors
// use jwt
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt")["Sign"])),
        ValidIssuer = configuration.GetSection("Jwt")["Issuer"],
        ValidAudience = configuration.GetSection("Jwt")["Audience"],
        ValidateIssuer = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true
    };
});

// use OpenIddict
//services.AddAuthentication(options =>
//{
//    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//});
//services.AddOpenIddict()
//    .AddValidation(options =>
//    {
//        options.SetIssuer("https://localhost:5001/");
//        options.UseIntrospection()
//            .SetClientId("api")
//            .SetClientSecret("myApiTestSecret");

//        options.UseSystemNetHttp();
//        options.UseAspNetCore();
//    });

// ��֤
services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireClaim("scope", "openid profile email offline_access");
    });
    options.AddPolicy("User", policy =>
        policy.RequireRole("Admin", "User"));
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
});

// cors���� 
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
// api �ӿ��ĵ�����
services.AddOpenApiDocument(c =>
{
    c.GenerateXmlObjects = true;
    c.GenerateEnumMappingDescription = true;
    c.UseControllerSummaryAsTagDescription = true;
    c.PostProcess = (document) =>
    {
        document.Info.Title = "Dev Platform";
        document.Info.Description = "Api �ĵ�";
        document.Info.Version = "1.0";
    };
});
services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCors("default");
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi3(c => { c.DocumentTitle = "�ĵ�"; });
    app.UseStaticFiles();
}
else
{
    // ����������Ҫ�µ�����
    app.UseCors("default");
    //app.UseHsts();
    app.UseHttpsRedirection();
}
// �쳣ͳһ����
//app.UseExceptionHandler(handler =>
//{
//    handler.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        var result = new
//        {
//            Title = "�����ڲ�����:" + exception?.Message,
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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public partial class Program { }