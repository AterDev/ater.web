﻿using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.Test;

public class BaseTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly IServiceProvider Services;
    public BaseTest(WebApplicationFactory<Program> factory)
    {
        Services = factory.Services.CreateScope().ServiceProvider;
    }
}
