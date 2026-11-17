using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Domain;
using Atlas.Forum.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Forum.Infrastructure.Repositories;

internal sealed class ForumThreadRepository(ForumDbContext db) : IForumThreadRepository
{
    public Task<ForumThread?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.Threads.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task<bool> ExistsForFindingAsync(Guid findingId, CancellationToken cancellationToken) =>
        db.Threads.AnyAsync(t => t.FindingId == findingId, cancellationToken);

    public void Add(ForumThread thread) => db.Threads.Add(thread);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
