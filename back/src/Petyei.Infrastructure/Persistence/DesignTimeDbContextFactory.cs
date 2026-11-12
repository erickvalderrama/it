using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Petyei.Infrastructure.Persistence;

/// <summary>Lets `dotnet ef` create migrations without starting the API.</summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PetyeiDbContext>
{
    public PetyeiDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("PETYEI_DESIGN_CONNECTION")
            ?? "Host=localhost;Database=petyei_design;Username=design;Password=design";
        var options = new DbContextOptionsBuilder<PetyeiDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;
        return new PetyeiDbContext(options);
    }
}
