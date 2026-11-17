using Atlas.Forum.Domain;

namespace Atlas.Forum.Application.Abstractions;

public interface IForumThreadRepository
{
    Task<ForumThread?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsForFindingAsync(Guid findingId, CancellationToken cancellationToken);

    void Add(ForumThread thread);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
