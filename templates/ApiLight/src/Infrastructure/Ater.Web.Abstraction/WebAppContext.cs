﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ater.Web.Abstraction;
/// <summary>
/// webapp上下文,静态，谨慎使用
/// </summary>
public static class WebAppContext
{
    /// <summary>
    /// 非必要情况下不要使用，尽量使用DI
    /// </summary>
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    public static IConfiguration Configuration { get; private set; } = null!;
    public static IWebHostEnvironment Environment { get; private set; } = null!;

    public static void SetWebAppContext(IServiceProvider serviceProvider, IConfiguration configuration, IWebHostEnvironment environment)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Environment = environment;
    }

    /// <summary>
    /// 获取新作用域服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetScopeService<T>()
    {
        return ServiceProvider.CreateScope().ServiceProvider.GetService<T>();
    }
}

public static partial class WebApplicationExtensions
{
    /// <summary>
    /// 使用静态上下文
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseWebAppContext(this WebApplication app)
    {
        WebAppContext.SetWebAppContext(app.Services, app.Configuration, app.Environment);
        return app;
    }
}