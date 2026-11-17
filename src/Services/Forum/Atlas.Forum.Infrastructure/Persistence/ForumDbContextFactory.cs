using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Atlas.Forum.Infrastructure.Persistence;

/// <summary>Używane tylko przez <c>dotnet ef</c> przy tworzeniu migracji.</summary>
internal sealed class ForumDbContextFactory : IDesignTimeDbContextFactory<ForumDbContext>
{
    public ForumDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<ForumDbContext>()
            .UseNpgsql("Host=localhost;Database=atlas_forum;Username=atlas;Password=atlas")
            .Options);
}
