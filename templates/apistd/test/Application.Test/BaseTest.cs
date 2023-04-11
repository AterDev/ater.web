using Application.Implement;
using Application.Interface;
using EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.Test;

public class BaseTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly IServiceProvider Services;
    public BaseTest(WebApplicationFactory<Program> factory)
    {
        factory = factory.WithWebHostBuilder(builder =>
          {
              builder.ConfigureServices(services =>
              {
                  services.AddTransient<IUserContext, TestUserContext>();
              });
          });
        Services = factory.Services.CreateScope().ServiceProvider;
    }
}
