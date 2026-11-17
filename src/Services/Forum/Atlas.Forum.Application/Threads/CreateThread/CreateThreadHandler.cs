using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Threads.CreateThread;

internal sealed class CreateThreadHandler(IForumThreadRepository threads, TimeProvider clock)
    : IRequestHandler<CreateThreadCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateThreadCommand command, CancellationToken cancellationToken)
    {
        var thread = ForumThread.Start(
            command.Title, command.Category, command.Content, command.AuthorId, command.AuthorName, clock.GetUtcNow());

        threads.Add(thread);
        await threads.SaveChangesAsync(cancellationToken);
        return thread.Id;
    }
}
