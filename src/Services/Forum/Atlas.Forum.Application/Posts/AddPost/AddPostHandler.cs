using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Forum.Application.Posts.AddPost;

internal sealed class AddPostHandler(IForumThreadRepository threads, TimeProvider clock) : IRequestHandler<AddPostCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddPostCommand command, CancellationToken cancellationToken)
    {
        var thread = await threads.FindAsync(command.ThreadId, cancellationToken);
        if (thread is null)
        {
            return ForumErrors.ThreadNotFound;
        }

        var post = thread.AddPost(command.Content, command.AuthorId, command.AuthorName, clock.GetUtcNow());
        if (post.IsFailed)
        {
            return post.ToResult();
        }

        await threads.SaveChangesAsync(cancellationToken);
        return post.Value.Id;
    }
}
