using Atlas.Kit.Domain;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Atlas.Kit.Api.Errors;

public static class ResultExtensions
{
    /// <summary>Sukces → <paramref name="onSuccess"/>, błąd → ProblemDetails z kodem wynikającym z <see cref="ErrorKind"/>.</summary>
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess) =>
        result.IsSuccess ? onSuccess(result.Value) : ToProblem(result.Errors);

    public static IResult ToHttpResult(this Result result, Func<IResult> onSuccess) =>
        result.IsSuccess ? onSuccess() : ToProblem(result.Errors);

    private static IResult ToProblem(IReadOnlyList<IError> errors)
    {
        var error = errors.OfType<AppError>().FirstOrDefault();
        var status = error?.Kind switch
        {
            ErrorKind.NotFound => StatusCodes.Status404NotFound,
            ErrorKind.Conflict => StatusCodes.Status409Conflict,
            ErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest,
        };

        return Results.Problem(
            title: error?.Message ?? errors.FirstOrDefault()?.Message ?? "Błąd",
            statusCode: status,
            extensions: new Dictionary<string, object?> { ["code"] = error?.Code });
    }
}
