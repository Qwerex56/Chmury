using FluentResults;

namespace Atlas.Kit.Domain;

/// <summary>Błąd aplikacji z kodem i rodzajem; zwracany w <see cref="Result"/> zamiast wyjątków.</summary>
public sealed class AppError : Error
{
    public AppError(string code, string message, ErrorKind kind)
        : base(message)
    {
        Code = code;
        Kind = kind;
    }

    public string Code { get; }

    public ErrorKind Kind { get; }

    public static AppError Validation(string code, string message) => new(code, message, ErrorKind.Validation);

    public static AppError NotFound(string code, string message) => new(code, message, ErrorKind.NotFound);

    public static AppError Conflict(string code, string message) => new(code, message, ErrorKind.Conflict);

    public static AppError Unauthorized(string code, string message) => new(code, message, ErrorKind.Unauthorized);
}
