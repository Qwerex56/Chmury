using System.Security.Claims;
using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Kit.Api.Security;
using Atlas.Users.Application.Accounts.GetCurrentUser;
using MediatR;

namespace Atlas.Users.Api.Endpoints;

internal sealed class GetCurrentUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/me", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetCurrentUserQuery(user.GetUserId()), ct);
                return result.ToHttpResult(view => Results.Ok(view));
            })
            .WithTags("Auth")
            .RequireAuthorization();
}
