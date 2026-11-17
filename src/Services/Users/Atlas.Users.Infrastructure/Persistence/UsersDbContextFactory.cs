using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Atlas.Users.Infrastructure.Persistence;

/// <summary>Używane tylko przez <c>dotnet ef</c> przy tworzeniu migracji.</summary>
internal sealed class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql("Host=localhost;Database=atlas_users;Username=atlas;Password=atlas")
            .Options);
}
