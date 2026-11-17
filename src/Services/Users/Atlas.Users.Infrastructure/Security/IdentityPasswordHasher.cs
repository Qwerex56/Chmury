using Atlas.Users.Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Infrastructure.Security;

/// <summary>PBKDF2 z ASP.NET Core Identity, bez reszty Identity.</summary>
internal sealed class IdentityPasswordHasher : IPasswordHasher
{
    private static readonly object Subject = new();
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(Subject, password);

    public bool Verify(string hash, string password) =>
        _hasher.VerifyHashedPassword(Subject, hash, password) != PasswordVerificationResult.Failed;
}
