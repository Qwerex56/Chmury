using Atlas.Users.Application.Common;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.GetCurrentUser;

/// <summary>Zwraca profil zalogowanego użytkownika (identyfikator z tokenu).</summary>
public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<Result<UserView>>;
