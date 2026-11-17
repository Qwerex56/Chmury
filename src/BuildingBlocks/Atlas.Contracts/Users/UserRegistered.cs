namespace Atlas.Contracts.Users;

/// <summary>Opublikowane przez serwis Użytkownicy po rejestracji nowego konta.</summary>
public sealed record UserRegistered(Guid UserId, string UserName, string Email, DateTimeOffset RegisteredAt);
