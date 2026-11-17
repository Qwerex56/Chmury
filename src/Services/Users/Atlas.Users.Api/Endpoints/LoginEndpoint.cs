using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Users.Application.Accounts.Login;
using MediatR;

namespace Atlas.Users.Api.Endpoints;

internal sealed class LoginEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/auth/login", async (LoginRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new LoginCommand(request.Email, request.Password), ct);
                return result.ToHttpResult(token => Results.Ok(token));
            })
            .WithTags("Auth")
            .AllowAnonymous();
}
