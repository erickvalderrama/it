namespace Petyei.Application.Common;

/// <summary>Outcome of a use case: either a value or a business error.</summary>
public sealed class Result<T>
{
    internal Result(T? value, AppError? error)
    {
        Value = value;
        Error = error;
    }

    public T? Value { get; }

    public AppError? Error { get; }

    public bool IsSuccess => Error is null;
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value, null);

    public static Result<T> Failure<T>(AppError error) => new(default, error);
}
