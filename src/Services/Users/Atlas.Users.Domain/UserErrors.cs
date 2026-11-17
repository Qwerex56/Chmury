using Atlas.Kit.Domain;

namespace Atlas.Users.Domain;

public static class UserErrors
{
    public static readonly AppError EmailTaken = AppError.Conflict("users.email_taken", "Ten adres e-mail jest już zarejestrowany.");

    public static readonly AppError UserNameTaken = AppError.Conflict("users.username_taken", "Ta nazwa użytkownika jest zajęta.");

    public static readonly AppError InvalidCredentials = AppError.Unauthorized("users.invalid_credentials", "Nieprawidłowy e-mail lub hasło.");

    public static readonly AppError NotFound = AppError.NotFound("users.not_found", "Nie znaleziono użytkownika.");
}
