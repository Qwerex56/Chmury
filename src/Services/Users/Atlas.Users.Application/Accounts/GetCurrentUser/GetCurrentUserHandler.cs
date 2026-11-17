using Atlas.Users.Application.Abstractions;
using Atlas.Users.Application.Common;
using Atlas.Users.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.GetCurrentUser;

internal sealed class GetCurrentUserHandler(IUserRepository users) : IRequestHandler<GetCurrentUserQuery, Result<UserView>>
{
    public async Task<Result<UserView>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await users.FindByIdAsync(query.UserId, cancellationToken);
        return user is null ? UserErrors.NotFound : UserView.From(user);
    }
}
