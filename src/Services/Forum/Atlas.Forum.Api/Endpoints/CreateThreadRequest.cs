using Atlas.Forum.Domain;

namespace Atlas.Forum.Api.Endpoints;

public sealed record CreateThreadRequest(string Title, ForumCategory Category, string Content);
