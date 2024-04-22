using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Ater.Web.Abstraction;

public static class ExceptionHandler
{
    public static Action<IApplicationBuilder> Handler()
    {
        return builder =>
        {
            builder.Run(async context =>
            {
                context.Response.StatusCode = 500;
                Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                var result = new ErrorResult
                {
                    Title = "异常错误",
                    Detail = exception?.Message + exception?.InnerException?.Message,
                    Status = 500,
                    TraceId = context.TraceIdentifier
                };
                await context.Response.WriteAsJsonAsync(result);
            });
        };
    }
}
