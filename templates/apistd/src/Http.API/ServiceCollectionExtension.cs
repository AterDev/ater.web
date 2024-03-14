using System.Net;
using System.Threading.RateLimiting;
using Ater.Web.Abstraction;
using Http.API;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Http.API;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// 添加web服务组件，如身份认证/授权/swagger/cors
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigWebComponents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwagger();
        services.AddJwtAuthentication(configuration);
        services.AddAuthorize();
        services.AddCors();
        services.AddRateLimiter();
        return services;
    }

    /// <summary>
    /// 添加速率限制
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            // 验证码  每10秒5次
            options.AddPolicy("captcha", context =>
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if (!IPAddress.IsLoopback(remoteIpAddress!))
                {
                    return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress!.ToString(), _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 3
                    });
                }
                else
                {
                    return RateLimitPartition.GetNoLimiter(remoteIpAddress!.ToString());
                }
            });

            // 全局限制 每10秒100次
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
            {
                IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

                if (!IPAddress.IsLoopback(remoteIpAddress!))
                {
                    return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress!, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 3
                    });
                }

                return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
            });
        });
        return services;
    }

    /// <summary>
    /// 添加 jwt 验证
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(cfg =>
        {
            cfg.SaveToken = true;
            var sign = configuration.GetSection("Authentication")["Jwt:Sign"];
            if (string.IsNullOrEmpty(sign))
            {
                throw new Exception("未找到有效的Jwt配置");
            }
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sign)),
                ValidIssuer = configuration.GetSection("Authentication")["Jwt:ValidIssuer"],
                ValidAudience = configuration.GetSection("Authentication")["Jwt:ValidAudiences"],
                ValidateIssuer = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true
            };
        });
        return services;
    }

    /// <summary>
    /// 添加swagger服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            c.SwaggerDoc("admin", new OpenApiInfo
            {
                Title = "MyProjectName",
                Description = "Admin API 文档. 更新时间:" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                Version = "v1"
            });
            c.SwaggerDoc("client", new OpenApiInfo
            {
                Title = "MyProjectName client",
                Description = "Client API 文档. 更新时间:" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                Version = "v1"
            });
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var item in xmlFiles)
            {
                try
                {
                    c.IncludeXmlComments(item, includeControllerXmlComments: true);
                }
                catch (Exception) { }
            }
            c.SupportNonNullableReferenceTypes();
            c.DescribeAllParametersInCamelCase();
            c.CustomOperationIds((z) =>
            {
                var descriptor = (ControllerActionDescriptor)z.ActionDescriptor;
                return $"{descriptor.ControllerName}_{descriptor.ActionName}";
            });
            c.SchemaFilter<EnumSchemaFilter>();
            c.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });
        });
        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        return services;
    }

    public static IServiceCollection AddAuthorize(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AppConst.User, policy => policy.RequireRole(AppConst.User))
            .AddPolicy(AppConst.AdminUser, policy => policy.RequireRole(AppConst.SuperAdmin, AppConst.AdminUser))
            .AddPolicy(AppConst.SuperAdmin, policy => policy.RequireRole(AppConst.SuperAdmin));
        return services;
    }
}
