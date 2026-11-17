using Atlas.Map.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Map.Infrastructure.Persistence;

public sealed class MapDbContext(DbContextOptions<MapDbContext> options) : DbContext(options)
{
    public DbSet<Finding> Findings => Set<Finding>();

    public DbSet<MushroomSpecies> Species => Set<MushroomSpecies>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("map");
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MapDbContext).Assembly);
    }
}
