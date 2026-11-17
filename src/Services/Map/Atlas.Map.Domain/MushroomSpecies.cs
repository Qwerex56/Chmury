namespace Atlas.Map.Domain;

/// <summary>Gatunek grzyba (dane słownikowe). Id to czytelny slug, np. "borowik-szlachetny".</summary>
public sealed class MushroomSpecies
{
    private MushroomSpecies()
    {
    }

    public string Id { get; private set; } = string.Empty;

    public string PolishName { get; private set; } = string.Empty;

    public string LatinName { get; private set; } = string.Empty;

    public Edibility Edibility { get; private set; }

    /// <summary>Miesiące owocnikowania (1–12), od których do których.</summary>
    public int SeasonFrom { get; private set; }

    public int SeasonTo { get; private set; }

    public string Color { get; private set; } = "#8d6e63";

    public static MushroomSpecies Create(
        string id, string polishName, string latinName, Edibility edibility, int seasonFrom, int seasonTo, string color) =>
        new()
        {
            Id = id,
            PolishName = polishName,
            LatinName = latinName,
            Edibility = edibility,
            SeasonFrom = seasonFrom,
            SeasonTo = seasonTo,
            Color = color,
        };

    public bool IsInSeason(int month) => month >= SeasonFrom && month <= SeasonTo;
}
