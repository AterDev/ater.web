using System;

namespace Http.API.Test;

public class BaseTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;

    protected readonly IServiceProvider _service;
    public BaseTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _service = _factory.Services.CreateScope().ServiceProvider;
    }

}
