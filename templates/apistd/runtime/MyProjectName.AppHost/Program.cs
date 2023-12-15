IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Http_API>("http.api");

builder.AddProject<Projects.StandaloneService>("standaloneservice");

builder.Build().Run();
