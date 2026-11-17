namespace Atlas.Contracts.Map;

/// <summary>Opublikowane przez serwis Mapa, gdy użytkownik doda znalezisko. Forum tworzy z niego wątek.</summary>
public sealed record FindingReported(
    Guid FindingId,
    string SpeciesName,
    double Latitude,
    double Longitude,
    string? Note,
    Guid AuthorId,
    string AuthorName,
    DateTimeOffset FoundAt);
