namespace Atlas.Map.Application.Common;

public sealed record FindingView(
    Guid Id,
    string SpeciesId,
    string SpeciesName,
    string Edibility,
    string Color,
    double Latitude,
    double Longitude,
    DateTimeOffset FoundAt,
    string? Note,
    string AuthorName);
