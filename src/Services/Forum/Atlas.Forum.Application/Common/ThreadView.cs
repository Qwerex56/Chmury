namespace Atlas.Forum.Application.Common;

public sealed record ThreadView(
    Guid Id,
    string Title,
    string Category,
    string AuthorName,
    Guid? FindingId,
    DateTimeOffset CreatedAt,
    IReadOnlyList<PostView> Posts);
