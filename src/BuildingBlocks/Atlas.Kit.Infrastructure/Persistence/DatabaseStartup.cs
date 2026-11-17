using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Atlas.Kit.Infrastructure.Persistence;

public static class DatabaseStartup
{
    /// <summary>
    /// Stosuje migracje EF Core i uruchamia seedery. Ponawia próby, bo w Docker Compose
    /// baza może jeszcze nie przyjmować połączeń.
    /// </summary>
    public static async Task MigrateDatabaseAsync<TContext>(this IHost host, CancellationToken cancellationToken = default)
        where TContext : DbContext
    {
        var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DatabaseStartup));

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await using var scope = host.Services.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                await context.Database.MigrateAsync(cancellationToken);

                foreach (var seeder in scope.ServiceProvider.GetServices<IDataSeeder>())
                {
                    await seeder.SeedAsync(cancellationToken);
                }

                logger.LogInformation("Baza {Context} gotowa", typeof(TContext).Name);
                return;
            }
            catch (Exception ex) when (attempt < 10)
            {
                logger.LogWarning(ex, "Baza niedostępna (próba {Attempt}/10), ponawiam za 3 s", attempt);
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            }
        }
    }
}
