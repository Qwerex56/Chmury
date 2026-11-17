using Atlas.Contracts.Users;
using Atlas.Kit.Application.Messaging;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Application.Common;
using Atlas.Users.Domain;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.Register;

internal sealed class RegisterHandler(
    IUserRepository users,
    IPasswordHasher hasher,
    ITokenIssuer tokens,
    IIntegrationEventPublisher events,
    TimeProvider clock) : IRequestHandler<RegisterCommand, Result<AuthTokenView>>
{
    public async Task<Result<AuthTokenView>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await users.EmailExistsAsync(command.Email.Trim().ToLowerInvariant(), cancellationToken))
        {
            return UserErrors.EmailTaken;
        }

        if (await users.UserNameExistsAsync(command.UserName.Trim(), cancellationToken))
        {
            return UserErrors.UserNameTaken;
        }

        var user = User.Register(command.Email, command.UserName, hasher.Hash(command.Password), clock.GetUtcNow());
        users.Add(user);
        await users.SaveChangesAsync(cancellationToken);

        await events.PublishAsync(new UserRegistered(user.Id, user.UserName, user.Email, user.CreatedAt), cancellationToken);

        return tokens.Issue(user);
    }
}
