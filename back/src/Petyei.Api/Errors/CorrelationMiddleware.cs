using System.Text.RegularExpressions;

namespace Petyei.Api.Errors;

/// <summary>
/// Accepts a safe <c>X-Correlation-Id</c> from the client or creates one, returns it in the response,
/// and adds it to the logging scope so support can trace a request without personal data.
/// </summary>
internal sealed partial class CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
{
    public const string HeaderName = "X-Correlation-Id";
    private const string ItemKey = "CorrelationId";

    public static string? Get(HttpContext context) => context.Items[ItemKey] as string;

    public async Task InvokeAsync(HttpContext context)
    {
        var incoming = context.Request.Headers[HeaderName].ToString();
        var correlationId = SafeId().IsMatch(incoming) ? incoming : Guid.NewGuid().ToString("N");

        context.Items[ItemKey] = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            await next(context).ConfigureAwait(false);
        }
    }

    [GeneratedRegex("^[A-Za-z0-9-]{8,64}$")]
    private static partial Regex SafeId();
}
