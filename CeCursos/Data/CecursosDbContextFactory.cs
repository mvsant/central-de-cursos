using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace cecursos.Data;

public sealed class CecursosDbContextFactory
    : IDesignTimeDbContextFactory<CecursosDbContext>
{
    public CecursosDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets(typeof(CecursosDbContext).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set ConnectionStrings:DefaultConnection using .NET User Secrets or an environment variable before running EF commands.");
        }

        var options = new DbContextOptionsBuilder<CecursosDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CecursosDbContext(options);
    }
}
