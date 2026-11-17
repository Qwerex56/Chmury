using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Application.Common;
using MediatR;

namespace Atlas.Forum.Application.Threads.GetThreads;

internal sealed class GetThreadsHandler(IForumReader reader) : IRequestHandler<GetThreadsQuery, IReadOnlyList<ThreadSummaryView>>
{
    public Task<IReadOnlyList<ThreadSummaryView>> Handle(GetThreadsQuery query, CancellationToken cancellationToken) =>
        reader.GetThreadsAsync(query.Category, cancellationToken);
}
