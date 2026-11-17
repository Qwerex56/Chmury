using Atlas.Map.Application.Abstractions;
using Atlas.Map.Application.Common;
using Atlas.Map.Domain;
using Atlas.Map.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Atlas.Map.Infrastructure.Readers;

internal sealed class MapReader(MapDbContext db, TimeProvider clock) : IMapReader
{
    private static readonly GeometryFactory Geometry = new(new PrecisionModel(), Finding.Srid);

    public async Task<IReadOnlyList<FindingView>> GetFindingsAsync(FindingFilter filter, CancellationToken cancellationToken)
    {
        var findings = db.Findings.AsNoTracking();

        if (filter.Area is { } area)
        {
            var box = Geometry.ToGeometry(new Envelope(area.West, area.East, area.South, area.North));
            findings = findings.Where(f => f.Location.Within(box));
        }

        if (!string.IsNullOrWhiteSpace(filter.SpeciesId))
        {
            findings = findings.Where(f => f.SpeciesId == filter.SpeciesId);
        }

        if (filter.LastDays is > 0)
        {
            var since = clock.GetUtcNow().AddDays(-filter.LastDays.Value);
            findings = findings.Where(f => f.FoundAt >= since);
        }

        var rows = await findings
            .OrderByDescending(f => f.FoundAt)
            .Take(500)
            .Join(db.Species, f => f.SpeciesId, s => s.Id, (f, s) => new { Finding = f, Species = s })
            .ToListAsync(cancellationToken);

        return rows
            .Select(r => new FindingView(
                r.Finding.Id,
                r.Species.Id,
                r.Species.PolishName,
                r.Species.Edibility.ToString(),
                r.Species.Color,
                r.Finding.Latitude,
                r.Finding.Longitude,
                r.Finding.FoundAt,
                r.Finding.Note,
                r.Finding.AuthorName))
            .ToList();
    }

    public async Task<IReadOnlyList<SpeciesView>> GetSpeciesAsync(CancellationToken cancellationToken) =>
        await db.Species.AsNoTracking()
            .OrderBy(s => s.PolishName)
            .Select(s => new SpeciesView(s.Id, s.PolishName, s.LatinName, s.Edibility.ToString(), s.SeasonFrom, s.SeasonTo, s.Color))
            .ToListAsync(cancellationToken);
}
