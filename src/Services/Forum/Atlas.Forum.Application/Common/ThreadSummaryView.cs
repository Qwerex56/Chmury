namespace Atlas.Forum.Application.Common;

public sealed record ThreadSummaryView(
    Guid Id,
    string Title,
    string Category,
    string AuthorName,
    int PostCount,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastActivityAt);
