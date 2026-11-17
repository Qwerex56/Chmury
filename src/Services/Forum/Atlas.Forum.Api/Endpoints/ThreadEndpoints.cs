using System.Security.Claims;
using Atlas.Forum.Application.Posts.AddPost;
using Atlas.Forum.Application.Threads.CreateThread;
using Atlas.Forum.Application.Threads.GetThread;
using Atlas.Forum.Application.Threads.GetThreads;
using Atlas.Forum.Domain;
using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Kit.Api.Security;
using MediatR;

namespace Atlas.Forum.Api.Endpoints;

internal sealed class ThreadEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/threads").WithTags("Threads");

        group.MapGet("/", async (ForumCategory? category, ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetThreadsQuery(category), ct)));

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            (await sender.Send(new GetThreadQuery(id), ct)).ToHttpResult(thread => Results.Ok(thread)));

        group.MapPost("/", async (CreateThreadRequest request, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(
                    new CreateThreadCommand(request.Title, request.Category, request.Content, user.GetUserId(), user.GetUserName()),
                    ct);
                return result.ToHttpResult(id => Results.Created($"/threads/{id}", new IdResponse(id)));
            })
            .RequireAuthorization();

        group.MapPost("/{id:guid}/posts", async (Guid id, AddPostRequest request, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new AddPostCommand(id, request.Content, user.GetUserId(), user.GetUserName()), ct);
                return result.ToHttpResult(postId => Results.Created($"/threads/{id}", new IdResponse(postId)));
            })
            .RequireAuthorization();
    }
}
