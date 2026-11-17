namespace Atlas.Users.Domain;

/// <summary>Konto grzybiarza. Hasło trzymamy wyłącznie jako hash.</summary>
public sealed class User
{
    private User()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string UserName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static User Register(string email, string userName, string passwordHash, DateTimeOffset now, UserRole role = UserRole.Member) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Email = email.Trim().ToLowerInvariant(),
            UserName = userName.Trim(),
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = now,
        };
}
