using Atlas.Map.Domain;

namespace Atlas.Map.Infrastructure.Seeding;

/// <summary>Startowy słownik gatunków. Te same identyfikatory zna serwis Predykcja.</summary>
internal static class SpeciesCatalog
{
    public static IReadOnlyList<MushroomSpecies> All { get; } =
    [
        MushroomSpecies.Create("borowik-szlachetny", "Borowik szlachetny", "Boletus edulis", Edibility.Jadalny, 6, 10, "#8d5524"),
        MushroomSpecies.Create("podgrzybek-brunatny", "Podgrzybek brunatny", "Imleria badia", Edibility.Jadalny, 7, 11, "#6d4c41"),
        MushroomSpecies.Create("kania", "Czubajka kania", "Macrolepiota procera", Edibility.Jadalny, 7, 10, "#bcaaa4"),
        MushroomSpecies.Create("kurka", "Pieprznik jadalny (kurka)", "Cantharellus cibarius", Edibility.Jadalny, 6, 10, "#f4a300"),
        MushroomSpecies.Create("maslak-zwyczajny", "Maślak zwyczajny", "Suillus luteus", Edibility.Jadalny, 6, 11, "#c49a2c"),
        MushroomSpecies.Create("kozlarz-babka", "Koźlarz babka", "Leccinum scabrum", Edibility.Jadalny, 6, 10, "#a1887f"),
        MushroomSpecies.Create("rydz", "Mleczaj rydz", "Lactarius deliciosus", Edibility.Jadalny, 8, 10, "#e65100"),
        MushroomSpecies.Create("opienka-miodowa", "Opieńka miodowa", "Armillaria mellea", Edibility.Jadalny, 9, 11, "#d4a017"),
        MushroomSpecies.Create("goryczak-zolciowy", "Goryczak żółciowy", "Tylopilus felleus", Edibility.Niejadalny, 6, 10, "#9e9d24"),
        MushroomSpecies.Create("muchomor-czerwony", "Muchomor czerwony", "Amanita muscaria", Edibility.Trujacy, 7, 11, "#d32f2f"),
        MushroomSpecies.Create("muchomor-sromotnikowy", "Muchomor sromotnikowy", "Amanita phalloides", Edibility.Trujacy, 7, 10, "#7cb342"),
    ];
}
