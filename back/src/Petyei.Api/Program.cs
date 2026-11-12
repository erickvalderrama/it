using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Petyei.Api.Commands;
using Petyei.Api.Endpoints;
using Petyei.Api.Errors;
using Petyei.Api.Security;
using Petyei.Application.Common;
using Petyei.Application;
using Petyei.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Extensions["correlationId"] = CorrelationMiddleware.Get(context.HttpContext);
    context.ProblemDetails.Extensions.TryAdd("code", context.ProblemDetails.Status switch
    {
        StatusCodes.Status404NotFound => "common.not_found",
        StatusCodes.Status405MethodNotAllowed => "common.method_not_allowed",
        >= 500 => "common.unexpected_error",
        _ => "common.request_error",
    });
});
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
        .AllowAnyHeader()
        .AllowAnyMethod()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentActor, HttpCurrentActor>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (ImportCatalogsCommand.IsRequested(args))
{
    return await ImportCatalogsCommand.RunAsync(app.Services, args).ConfigureAwait(false);
}

app.UseMiddleware<CorrelationMiddleware>();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Liveness only reports that the process runs; readiness checks dependencies with bounded timeouts.
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains(Petyei.Infrastructure.DependencyInjection.ReadinessTag) });
app.MapGroup("/api/v1").MapLocationEndpoints();

await app.RunAsync().ConfigureAwait(false);
return 0;

public partial class Program;
