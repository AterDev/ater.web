using Http.API.BackgroundTask;
using Share.Azure;
using Share.NewsCollectionService;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddHostedService<NewsTimerService>();
services.AddHttpContextAccessor();
services.Configure<AzureOptions>(configuration.GetSection("Azure"));
var connectionString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContextPool<ContextBase>(option =>
{
    option.UseNpgsql(connectionString, sql => { sql.MigrationsAssembly("EntityFramework.Migrator"); });
});


services.AddOptions();
services.AddScoped<IUserContext, UserContext>();
services.AddScoped<NewsCollectionService>();
services.AddScoped<TwitterService>();
services.AddScoped(typeof(FileService));

services.AddDataStore();

#region �ӿ��������:jwt/��Ȩ/cors
// jwt
services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer("https://localhost:5001/");
        options.UseIntrospection()
            .SetClientId("api")
            .SetClientSecret("myApiTestSecret");

        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });
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

app.UseHealthChecks("/health");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();