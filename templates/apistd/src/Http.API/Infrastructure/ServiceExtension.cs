using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Http.API.Infrastructure;

public static class ServiceExtension
{
    /// <summary>
    /// 添加opentelemetry 服务及选项
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceName"></param>
    /// <param name="otlpOptions"></param>
    /// <param name="loggerOptions"></param>
    /// <param name="tracerProvider"></param>
    /// <param name="meterProvider"></param>
    public static void AddOpenTelemetry(this IServiceCollection services,
        string serviceName,
        Action<OtlpExporterOptions>? otlpOptions = null,
        Action<OpenTelemetryLoggerOptions>? loggerOptions = null,
        Action<TracerProviderBuilder>? tracerProvider = null,
        Action<MeterProviderBuilder>? meterProvider = null)
    {
        var resource = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceInstanceId: Environment.MachineName);

        otlpOptions ??= new Action<OtlpExporterOptions>(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:4317");
        });

        loggerOptions ??= new Action<OpenTelemetryLoggerOptions>(options =>
        {
            options.SetResourceBuilder(resource);
            options.AddOtlpExporter(otlpOptions);
            options.ParseStateValues = true;
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
#if DEBUG
            options.AddConsoleExporter();
#endif
        });
        // 返回内容最大长度截取
        int maxLength = 2048;
        tracerProvider ??= new Action<TracerProviderBuilder>(options =>
        {
            options.AddSource(serviceName)
                .SetResourceBuilder(resource)
                .AddHttpClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.EnrichWithHttpRequestMessage = async (activity, httpRequestMessage) =>
                    {
                        if (httpRequestMessage.Content != null)
                        {
                            var stream = httpRequestMessage.Content.ReadAsStream();
                            var body = await new StreamReader(stream).ReadToEndAsync();
                            activity.SetTag("requestBody", body);
                        }
                    };

                    options.EnrichWithHttpResponseMessage = (activity, httpResponseMessage) =>
                    {
                        if (httpResponseMessage.Content != null)
                        {
                            // 不要使用ReadAsStream;此处需要同步执行;否则会出错
                            var body = httpResponseMessage.Content.ReadAsStringAsync().Result;
                            body = body.Length > maxLength ? body[0..maxLength] : body;
                            activity.SetTag("responseBody", body);
                        }
                    };
                    options.EnrichWithException = (activity, exception) =>
                    {
                    };
                })
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.EnrichWithHttpRequest = async (activity, request) =>
                    {
                        request.EnableBuffering();
                        request.Body.Position = 0;
                        var reader = new StreamReader(request.Body);
                        activity.SetTag("requestBody", await reader.ReadToEndAsync());
                        request.Body.Position = 0;
                    };

                    options.EnrichWithHttpResponse = (activity, response) =>
                    {
                    };

                    options.EnrichWithException = (activity, exception) =>
                    {
                        activity.SetTag("stackTrace", exception.StackTrace);
                        activity.SetTag("message", exception.Message);
                    };
                })
                .AddSqlClientInstrumentation()
#if DEBUG
            .AddConsoleExporter()
#endif
                .AddOtlpExporter(otlpOptions);
        });

        meterProvider = new Action<MeterProviderBuilder>(options =>
        {
            options.AddMeter(serviceName)
                .SetResourceBuilder(resource)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(otlpOptions);
        });

        services.AddLogging(loggerBuilder =>
        {
            loggerBuilder.ClearProviders();
            loggerBuilder.AddOpenTelemetry(loggerOptions);
        });
        services.AddOpenTelemetry()
            .WithTracing(tracerProvider)
            .WithMetrics(meterProvider)
            .StartWithHost();
    }
}
