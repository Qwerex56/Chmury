using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Posts.AddPost;

/// <summary>Dodaje odpowiedź do istniejącego wątku i przesuwa jego ostatnią aktywność.</summary>
public sealed record AddPostCommand(Guid ThreadId, string Content, Guid AuthorId, string AuthorName) : IRequest<Result<Guid>>;
