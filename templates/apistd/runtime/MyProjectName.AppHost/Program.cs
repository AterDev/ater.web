IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Http_API>("http.api");

// add other services

builder.Build().Run();
