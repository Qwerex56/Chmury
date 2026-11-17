using Atlas.Users.Application.Common;
using FluentResults;
using MediatR;

namespace Atlas.Users.Application.Accounts.Register;

/// <summary>
/// Rejestruje nowe konto i od razu zwraca token. E-mail i nazwa użytkownika muszą być unikalne;
/// po sukcesie publikowane jest zdarzenie <c>UserRegistered</c>.
/// </summary>
public sealed record RegisterCommand(string Email, string UserName, string Password) : IRequest<Result<AuthTokenView>>;
