var builder = WebApplication.CreateBuilder(args);

// if use AppHost then use this
// builder.AddServiceDefaults();

IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

// 1 �������
services.AddAppComponents(configuration);
services.AddWebComponents(configuration);

// 2 ��Ȩ����
services.AddAuthorizationBuilder()
    .AddPolicy(AppConst.User, policy => policy.RequireRole(AppConst.User))
    .AddPolicy(AppConst.AdminUser, policy => policy.RequireRole(AppConst.SuperAdmin, AppConst.AdminUser))
    .AddPolicy(AppConst.SuperAdmin, policy => policy.RequireRole(AppConst.SuperAdmin));

services.AddHttpContextAccessor();
services.AddTransient<IUserContext, UserContext>();
services.AddTransient<ITenantProvider, TenantProvider>();
// 3 ���ݼ�ҵ��ӿ�ע��
services.AddManager();
// 4 �����Զ���ѡ�����
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

var app = builder.Build();

// if use AppHost then use this
// app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseCors("default");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/service/swagger.json", name: "service");
    });
}
else
{
    app.UseCors("default");
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
// �쳣ͳһ����
app.UseExceptionHandler(ExceptionHandler.Handler());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
