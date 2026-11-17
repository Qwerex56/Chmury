namespace Atlas.Map.Api.Endpoints;

public sealed record ReportFindingRequest(
    string SpeciesId,
    double Latitude,
    double Longitude,
    DateTimeOffset? FoundAt,
    string? Note);
