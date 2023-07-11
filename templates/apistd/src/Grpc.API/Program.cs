
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDataStore();
builder.Services.AddManager();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
"To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();