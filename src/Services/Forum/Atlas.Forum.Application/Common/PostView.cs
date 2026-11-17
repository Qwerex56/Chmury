namespace Atlas.Forum.Application.Common;

public sealed record PostView(Guid Id, string Content, string AuthorName, DateTimeOffset CreatedAt);
