using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Map.Domain;
using Atlas.Map.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Map.Infrastructure.Seeding;

/// <summary>Słownik gatunków + przykładowe znaleziska w znanych polskich lasach (deterministycznie, seed 42).</summary>
internal sealed class DemoMapSeeder(MapDbContext db, TimeProvider clock) : IDataSeeder
{
    private static readonly (string Name, double Lat, double Lon)[] Forests =
    [
        ("Puszcza Kampinoska", 52.33, 20.55),
        ("Bory Tucholskie", 53.75, 17.80),
        ("Puszcza Białowieska", 52.72, 23.85),
        ("Bieszczady", 49.25, 22.45),
        ("Bory Dolnośląskie", 51.40, 15.30),
        ("Puszcza Notecka", 52.75, 16.20),
        ("Lasy Janowskie", 50.65, 22.40),
    ];

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!await db.Species.AnyAsync(cancellationToken))
        {
            db.Species.AddRange(SpeciesCatalog.All);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (await db.Findings.AnyAsync(cancellationToken))
        {
            return;
        }

        var random = new Random(42);
        var now = clock.GetUtcNow();
        var species = SpeciesCatalog.All;

        foreach (var forest in Forests)
        {
            for (var i = 0; i < 9; i++)
            {
                var kind = species[random.Next(species.Count)];
                var finding = Finding.Report(
                    kind.Id,
                    forest.Lat + ((random.NextDouble() - 0.5) * 0.25),
                    forest.Lon + ((random.NextDouble() - 0.5) * 0.4),
                    now.AddDays(-random.Next(0, 30)).AddHours(-random.Next(0, 12)),
                    $"{forest.Name}: przykładowe znalezisko",
                    Guid.Empty,
                    "Atlas",
                    now);

                db.Findings.Add(finding.Value);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
