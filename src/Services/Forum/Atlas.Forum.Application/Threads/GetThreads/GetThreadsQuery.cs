using Atlas.Forum.Application.Common;
using Atlas.Forum.Domain;
using MediatR;

namespace Atlas.Forum.Application.Threads.GetThreads;

/// <summary>Lista wątków (opcjonalnie z jednej kategorii), od najnowszej aktywności.</summary>
public sealed record GetThreadsQuery(ForumCategory? Category) : IRequest<IReadOnlyList<ThreadSummaryView>>;
