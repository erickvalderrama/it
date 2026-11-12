using Petyei.Application.Locations;

namespace Petyei.Api.Endpoints;

internal static class LocationEndpoints
{
    public static RouteGroupBuilder MapLocationEndpoints(this RouteGroupBuilder api)
    {
        var locations = api.MapGroup("/locations").WithTags("Locations");
        locations.MapGet("/countries", async (ListEnabledCountries useCase, CancellationToken cancellationToken) =>
                TypedResults.Ok(await useCase.ExecuteAsync(cancellationToken).ConfigureAwait(false)))
            .WithName("ListEnabledCountries");
        return api;
    }
}
