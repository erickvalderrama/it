using Microsoft.Extensions.DependencyInjection;
using Petyei.Application.Locations;

namespace Petyei.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ListEnabledCountries>();
        return services;
    }
}
