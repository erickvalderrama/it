using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Petyei.Application.Catalogs;
using Petyei.Application.Locations;
using Petyei.Infrastructure.Catalogs;
using Petyei.Infrastructure.Locations;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure;

public static class DependencyInjection
{
    public const string ConnectionStringName = "Petyei";
    public const string ReadinessTag = "ready";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbLocationCatalog>();
        services.AddScoped<ICountryCatalog>(provider => provider.GetRequiredService<DbLocationCatalog>());
        services.AddScoped<ILocalityCatalog>(provider => provider.GetRequiredService<DbLocationCatalog>());

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<ICatalogStore, CatalogStore>();
        services.AddDbContext<PetyeiDbContext>((provider, options) => options
            .UseNpgsql(configuration.GetConnectionString(ConnectionStringName))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(provider.GetRequiredService<SoftDeleteInterceptor>()));

        var readiness = services.AddHealthChecks();
        if (string.IsNullOrWhiteSpace(configuration.GetConnectionString(ConnectionStringName)))
        {
            readiness.AddCheck(
                "database",
                () => HealthCheckResult.Unhealthy("Database connection is not configured."),
                tags: [ReadinessTag]);
        }
        else
        {
            readiness.AddDbContextCheck<PetyeiDbContext>("database", tags: [ReadinessTag]);
        }

        return services;
    }
}
