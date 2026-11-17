using Atlas.Map.Domain;

namespace Atlas.Map.Application.Abstractions;

public interface IFindingRepository
{
    Task<MushroomSpecies?> FindSpeciesAsync(string speciesId, CancellationToken cancellationToken);

    void Add(Finding finding);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
