namespace Atlas.Forum.Domain;

/// <summary>Odpowiedź w wątku. Tworzona tylko przez <see cref="ForumThread.AddPost"/>.</summary>
public sealed class Post
{
    private Post()
    {
    }

    internal Post(Guid threadId, string content, Guid authorId, string authorName, DateTimeOffset createdAt)
    {
        Id = Guid.CreateVersion7();
        ThreadId = threadId;
        Content = content;
        AuthorId = authorId;
        AuthorName = authorName;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Guid ThreadId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public Guid AuthorId { get; private set; }

    public string AuthorName { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; private set; }
}
