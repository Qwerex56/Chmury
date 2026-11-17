using Atlas.Forum.Application.Common;
using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Threads.GetThread;

/// <summary>Wątek ze wszystkimi postami w kolejności dodania.</summary>
public sealed record GetThreadQuery(Guid ThreadId) : IRequest<Result<ThreadView>>;
