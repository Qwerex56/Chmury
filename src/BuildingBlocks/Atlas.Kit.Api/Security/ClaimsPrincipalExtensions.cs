using System.Security.Claims;

namespace Atlas.Kit.Api.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new InvalidOperationException("Brak identyfikatora użytkownika w tokenie."));

    public static string GetUserName(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Name) ?? "nieznany";
}
