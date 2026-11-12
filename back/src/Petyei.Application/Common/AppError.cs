namespace Petyei.Application.Common;

public enum ErrorKind
{
    Validation,
    NotFound,
    Conflict,
    Forbidden,
}

/// <summary>
/// A business error with a stable code. Clients translate the code; the API never returns
/// user-facing sentences.
/// </summary>
public sealed record AppError(string Code, ErrorKind Kind, string? Field = null);
