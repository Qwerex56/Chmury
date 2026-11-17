using Atlas.Forum.Application.Common;
using Atlas.Forum.Domain;

namespace Atlas.Forum.Application.Abstractions;

/// <summary>Odczyty bez śledzenia zmian, prosto do widoków.</summary>
public interface IForumReader
{
    Task<IReadOnlyList<ThreadSummaryView>> GetThreadsAsync(ForumCategory? category, CancellationToken cancellationToken);

    Task<ThreadView?> GetThreadAsync(Guid id, CancellationToken cancellationToken);
}
