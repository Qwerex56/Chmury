using Atlas.Users.Application.Abstractions;
using Atlas.Users.Domain;
using Atlas.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Users.Infrastructure.Repositories;

internal sealed class UserRepository(UsersDbContext db) : IUserRepository
{
    public Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken) =>
        db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken) =>
        db.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken) =>
        db.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

    public void Add(User user) => db.Users.Add(user);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
