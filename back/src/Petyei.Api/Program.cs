using Petyei.Api.Endpoints;
using Petyei.Api.Security;
using Petyei.Application.Common;
using Petyei.Application;
using Petyei.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
        .AllowAnyHeader()
        .AllowAnyMethod()));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentActor, HttpCurrentActor>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health/live");
app.MapGroup("/api/v1").MapLocationEndpoints();

await app.RunAsync().ConfigureAwait(false);

public partial class Program;
