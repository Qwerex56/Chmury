using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Atlas.Map.Infrastructure.Persistence;

/// <summary>Używane tylko przez <c>dotnet ef</c> przy tworzeniu migracji.</summary>
internal sealed class MapDbContextFactory : IDesignTimeDbContextFactory<MapDbContext>
{
    public MapDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<MapDbContext>()
            .UseNpgsql("Host=localhost;Database=atlas_map;Username=atlas;Password=atlas", o => o.UseNetTopologySuite())
            .Options);
}
