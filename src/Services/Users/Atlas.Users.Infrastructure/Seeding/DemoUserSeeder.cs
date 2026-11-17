using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain;
using Atlas.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Users.Infrastructure.Seeding;

/// <summary>Konto demo do prezentacji: demo@atlas.pl / Grzybek123!</summary>
internal sealed class DemoUserSeeder(UsersDbContext db, IPasswordHasher hasher, TimeProvider clock) : IDataSeeder
{
    public const string DemoEmail = "demo@atlas.pl";
    public const string DemoPassword = "Grzybek123!";

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        db.Users.Add(User.Register(DemoEmail, "Borowik", hasher.Hash(DemoPassword), clock.GetUtcNow(), UserRole.Moderator));
        await db.SaveChangesAsync(cancellationToken);
    }
}
