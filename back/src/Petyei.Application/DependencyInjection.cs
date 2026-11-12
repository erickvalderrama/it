using Microsoft.Extensions.DependencyInjection;
using Petyei.Application.Catalogs;
using Petyei.Application.Locations;

namespace Petyei.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ListEnabledCountries>();
        services.AddScoped<GetEnabledCountry>();
        services.AddScoped<ListSelectableLocalities>();
        services.AddScoped<GetSelectableLocality>();
        services.AddScoped<ImportCatalogs>();
        return services;
    }
}
