using Atlas.Kit.Domain;

namespace Atlas.Map.Domain;

public static class MapErrors
{
    public static readonly AppError InvalidLocation = AppError.Validation("map.invalid_location", "Nieprawidłowe współrzędne.");

    public static readonly AppError FoundInFuture = AppError.Validation("map.found_in_future", "Data znaleziska nie może być z przyszłości.");

    public static readonly AppError UnknownSpecies = AppError.NotFound("map.unknown_species", "Nieznany gatunek grzyba.");
}
