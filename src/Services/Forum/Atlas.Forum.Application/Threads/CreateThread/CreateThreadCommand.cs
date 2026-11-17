using Atlas.Forum.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Threads.CreateThread;

/// <summary>Zakłada nowy wątek z pierwszym postem. Wymaga zalogowanego autora.</summary>
public sealed record CreateThreadCommand(
    string Title,
    ForumCategory Category,
    string Content,
    Guid AuthorId,
    string AuthorName) : IRequest<Result<Guid>>;
