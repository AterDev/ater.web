using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configBuilder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (!string.IsNullOrEmpty(env)) {
    configBuilder.AddJsonFile($"appsettings.{env}.json", true, true);
}
var config = configBuilder.Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureDefaults(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ContextBase>(option =>
        {
            var connectionString = config.GetConnectionString("Default");
            option.UseNpgsql(connectionString, option => option.MigrationsAssembly("EntityFramework.Migrator"));
        });
    });
host.RunConsoleAsync();
