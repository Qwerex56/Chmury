using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Application.Common;
using Atlas.Forum.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Threads.GetThread;

internal sealed class GetThreadHandler(IForumReader reader) : IRequestHandler<GetThreadQuery, Result<ThreadView>>
{
    public async Task<Result<ThreadView>> Handle(GetThreadQuery query, CancellationToken cancellationToken)
    {
        var thread = await reader.GetThreadAsync(query.ThreadId, cancellationToken);
        return thread is null ? ForumErrors.ThreadNotFound : thread;
    }
}
