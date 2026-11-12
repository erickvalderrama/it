using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petyei.Application.Locations;
using Petyei.Infrastructure.Locations;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure;

public static class DependencyInjection
{
    public const string ConnectionStringName = "Petyei";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CountryCatalogOptions>()
            .Bind(configuration.GetSection(CountryCatalogOptions.SectionName))
            .Validate(options => options.Enabled.Count > 0, "At least one enabled country is required.")
            .ValidateOnStart();
        services.AddSingleton<ICountryCatalog, ConfiguredCountryCatalog>();

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddDbContext<PetyeiDbContext>((provider, options) => options
            .UseNpgsql(configuration.GetConnectionString(ConnectionStringName))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(provider.GetRequiredService<SoftDeleteInterceptor>()));
        return services;
    }
}
