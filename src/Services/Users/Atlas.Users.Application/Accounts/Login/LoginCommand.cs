using Atlas.Users.Application.Common;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.Login;

/// <summary>Loguje użytkownika e-mailem i hasłem; zwraca token JWT. Błędne dane dają zawsze ten sam komunikat.</summary>
public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokenView>>;
