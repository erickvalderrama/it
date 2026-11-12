using Petyei.Application.Common;

namespace Petyei.Api.Errors;

/// <summary>Maps business errors to RFC 9457 problem details with a stable <c>code</c> extension.</summary>
internal static class ProblemResults
{
    public static IResult ToProblem(this AppError error)
    {
        var (status, title) = error.Kind switch
        {
            ErrorKind.Validation => (StatusCodes.Status400BadRequest, "Validation failed"),
            ErrorKind.NotFound => (StatusCodes.Status404NotFound, "Resource not found"),
            ErrorKind.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
            ErrorKind.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden"),
            _ => (StatusCodes.Status500InternalServerError, "Unexpected error"),
        };

        var extensions = new Dictionary<string, object?> { ["code"] = error.Code };
        if (error.Field is not null)
        {
            extensions["field"] = error.Field;
        }

        return TypedResults.Problem(statusCode: status, title: title, extensions: extensions);
    }
}
