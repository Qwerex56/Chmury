using Atlas.Kit.Domain;
using FluentResults;

namespace Atlas.Forum.Domain;

/// <summary>Wątek forum. Pierwszy post to treść wątku; kolejne dodaje <see cref="AddPost"/>.</summary>
public sealed class ForumThread
{
    private readonly List<Post> _posts = [];

    private ForumThread()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public ForumCategory Category { get; private set; }

    public Guid AuthorId { get; private set; }

    public string AuthorName { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset LastActivityAt { get; private set; }

    /// <summary>Znalezisko z mapy, z którego powstał wątek (tylko kategoria Znaleziska).</summary>
    public Guid? FindingId { get; private set; }

    public IReadOnlyList<Post> Posts => _posts;

    public static ForumThread Start(
        string title,
        ForumCategory category,
        string content,
        Guid authorId,
        string authorName,
        DateTimeOffset now,
        Guid? findingId = null)
    {
        var thread = new ForumThread
        {
            Id = Guid.CreateVersion7(),
            Title = title.Trim(),
            Category = category,
            AuthorId = authorId,
            AuthorName = authorName,
            CreatedAt = now,
            LastActivityAt = now,
            FindingId = findingId,
        };
        thread._posts.Add(new Post(thread.Id, content.Trim(), authorId, authorName, now));
        return thread;
    }

    public Result<Post> AddPost(string content, Guid authorId, string authorName, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return ForumErrors.EmptyPost;
        }

        var post = new Post(Id, content.Trim(), authorId, authorName, now);
        _posts.Add(post);
        LastActivityAt = now;
        return post;
    }
}
