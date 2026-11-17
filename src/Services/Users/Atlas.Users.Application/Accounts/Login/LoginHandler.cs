using Atlas.Users.Application.Abstractions;
using Atlas.Users.Application.Common;
using Atlas.Users.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.Login;

internal sealed class LoginHandler(IUserRepository users, IPasswordHasher hasher, ITokenIssuer tokens)
    : IRequestHandler<LoginCommand, Result<AuthTokenView>>
{
    public async Task<Result<AuthTokenView>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await users.FindByEmailAsync(command.Email.Trim().ToLowerInvariant(), cancellationToken);

        if (user is null || !hasher.Verify(user.PasswordHash, command.Password))
        {
            return UserErrors.InvalidCredentials;
        }

        return tokens.Issue(user);
    }
}
