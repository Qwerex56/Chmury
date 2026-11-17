using Atlas.Map.Application.Abstractions;
using Atlas.Map.Domain;
using Atlas.Map.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Map.Infrastructure.Repositories;

internal sealed class FindingRepository(MapDbContext db) : IFindingRepository
{
    public Task<MushroomSpecies?> FindSpeciesAsync(string speciesId, CancellationToken cancellationToken) =>
        db.Species.FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

    public void Add(Finding finding) => db.Findings.Add(finding);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
