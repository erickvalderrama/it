using Petyei.Api.Errors;
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

        locations.MapGet("/countries/{code}", async (string code, GetEnabledCountry useCase, CancellationToken cancellationToken) =>
            {
                var result = await useCase.ExecuteAsync(code, cancellationToken).ConfigureAwait(false);
                return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToProblem();
            })
            .WithName("GetEnabledCountry")
            .Produces<CountryDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        locations.MapGet("/countries/{countryCode}/localities", async (
                string countryCode,
                ListSelectableLocalities useCase,
                HttpContext http,
                CancellationToken cancellationToken) =>
            {
                var result = await useCase.ExecuteAsync(countryCode, cancellationToken).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    return result.Error!.ToProblem();
                }

                // The catalog changes only with an import; let clients and proxies reuse it briefly.
                http.Response.Headers.CacheControl = "public, max-age=3600";
                return TypedResults.Ok(result.Value);
            })
            .WithName("ListSelectableLocalities")
            .Produces<IReadOnlyList<DepartmentLocalitiesDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        locations.MapGet("/countries/{countryCode}/localities/{localityCode}", async (
                string countryCode,
                string localityCode,
                GetSelectableLocality useCase,
                CancellationToken cancellationToken) =>
            {
                var result = await useCase.ExecuteAsync(countryCode, localityCode, cancellationToken).ConfigureAwait(false);
                return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToProblem();
            })
            .WithName("GetSelectableLocality")
            .Produces<LocalityDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return api;
    }
}
