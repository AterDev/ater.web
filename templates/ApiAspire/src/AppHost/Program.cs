var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Http_API>("http-api");

builder.Build().Run();
