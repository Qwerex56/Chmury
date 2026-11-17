using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Application.Common;
using Atlas.Forum.Domain;
using Atlas.Forum.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Forum.Infrastructure.Readers;

internal sealed class ForumReader(ForumDbContext db) : IForumReader
{
    public async Task<IReadOnlyList<ThreadSummaryView>> GetThreadsAsync(ForumCategory? category, CancellationToken cancellationToken)
    {
        var query = db.Threads.AsNoTracking();
        if (category is not null)
        {
            query = query.Where(t => t.Category == category);
        }

        return await query
            .OrderByDescending(t => t.LastActivityAt)
            .Take(100)
            .Select(t => new ThreadSummaryView(
                t.Id, t.Title, t.Category.ToString(), t.AuthorName, t.Posts.Count, t.CreatedAt, t.LastActivityAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ThreadView?> GetThreadAsync(Guid id, CancellationToken cancellationToken)
    {
        var thread = await db.Threads.AsNoTracking()
            .Include(t => t.Posts)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return thread is null
            ? null
            : new ThreadView(
                thread.Id,
                thread.Title,
                thread.Category.ToString(),
                thread.AuthorName,
                thread.FindingId,
                thread.CreatedAt,
                thread.Posts
                    .OrderBy(p => p.CreatedAt)
                    .Select(p => new PostView(p.Id, p.Content, p.AuthorName, p.CreatedAt))
                    .ToList());
    }
}
