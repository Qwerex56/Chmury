namespace Atlas.Kit.Infrastructure.Security;

/// <summary>Ustawienia JWT wspólne dla wszystkich serwisów (sekcja "Jwt"). Klucz podpisu musi mieć min. 32 znaki.</summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "atlas-users";

    public string Audience { get; init; } = "atlas";

    public string SigningKey { get; init; } = string.Empty;

    public int LifetimeMinutes { get; init; } = 120;
}
