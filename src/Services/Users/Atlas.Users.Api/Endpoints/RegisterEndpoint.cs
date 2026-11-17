using Atlas.Kit.Api.Endpoints;
using Atlas.Kit.Api.Errors;
using Atlas.Users.Application.Accounts.Register;
using MediatR;

namespace Atlas.Users.Api.Endpoints;

internal sealed class RegisterEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/auth/register", async (RegisterRequest request, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new RegisterCommand(request.Email, request.UserName, request.Password), ct);
                return result.ToHttpResult(token => Results.Ok(token));
            })
            .WithTags("Auth")
            .AllowAnonymous();
}
