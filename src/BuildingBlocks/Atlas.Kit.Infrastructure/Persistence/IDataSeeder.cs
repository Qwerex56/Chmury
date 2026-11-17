namespace Atlas.Kit.Infrastructure.Persistence;

/// <summary>Wypełnia bazę danymi demonstracyjnymi po migracji (tylko gdy jest pusta).</summary>
public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}
