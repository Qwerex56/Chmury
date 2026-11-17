namespace Atlas.Map.Application.Common;

public sealed record SpeciesView(
    string Id,
    string PolishName,
    string LatinName,
    string Edibility,
    int SeasonFrom,
    int SeasonTo,
    string Color);
