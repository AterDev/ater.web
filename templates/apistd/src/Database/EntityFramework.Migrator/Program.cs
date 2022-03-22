using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureDefaults(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ContextBase>(option =>
        {
            var connectionString = config.GetConnectionString("Default");
            option.UseNpgsql(connectionString, option => option.MigrationsAssembly("EntityFramework.Migrator"));
        });
        services.AddDbContext<IdentityContext>(option =>
        {
            var connectionString = config.GetConnectionString("Identity");
            option.UseNpgsql(connectionString, option => option.MigrationsAssembly("EntityFramework.Migrator"));
        });
    });

host.RunConsoleAsync();
