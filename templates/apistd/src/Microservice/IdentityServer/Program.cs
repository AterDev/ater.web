using EntityFramework;
using IdentityServer;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// dbcontext
var connectionString = configuration.GetConnectionString("DefaultConnection");
var identityConnection = configuration.GetConnectionString("IdentityConnection");

services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(identityConnection).UseOpenIddict<Guid>());

services.AddDbContext<ContextBase>(opt => opt.UseNpgsql(connectionString));

// identity
services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ContextBase>()
    .AddDefaultTokenProviders();

services.AddSingleton<IEmailSender, EmailSender>();
// openid
services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;
    //options.SignIn.RequireConfirmedAccount = true;
});

// 自定义路径
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Loginout";
});


services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<IdentityContext>()
            .ReplaceDefaultEntities<Guid>();
        options.UseQuartz();
    })
    .AddServer(options =>
    {
        // enable endpoints
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetLogoutEndpointUris("/connect/logout")
            .SetIntrospectionEndpointUris("/connect/introspect")
            .SetTokenEndpointUris("/connect/token")
            .SetUserinfoEndpointUris("/connect/userinfo")
            .SetVerificationEndpointUris("/connect/verify");

        // enable flows
        options.AllowAuthorizationCodeFlow()
            .AllowHybridFlow()
            //.AllowPasswordFlow()
            //.AllowClientCredentialsFlow()
            .AllowRefreshTokenFlow();

        // register scopes
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, Scopes.OpenId);

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough()
            .EnableStatusCodePagesIntegration();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// cors
services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                .WithOrigins("https://localhost:4200")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

services.AddControllersWithViews();
services.AddRazorPages();
var app = builder.Build();

// 以下数据库初始化内容，可使用其他方式替代
await using (var scope = app.Services.CreateAsyncScope())
{
    // 生成数据库结构
    var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    context.Database.EnsureCreated();

    // 添加默认数据
    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    if (await manager.FindByClientIdAsync(InitClient.AdminClient.ClientId!) == null)
    {
        await manager.CreateAsync(InitClient.AdminClient);
    }
    if (await manager.FindByClientIdAsync(InitClient.Api.ClientId!) == null)
    {
        await manager.CreateAsync(InitClient.Api);
    }
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});
app.Run();
