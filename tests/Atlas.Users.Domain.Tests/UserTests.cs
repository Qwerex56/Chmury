using Atlas.Users.Domain;

namespace Atlas.Users.Domain.Tests;

public sealed class UserTests
{
    [Fact]
    public void Register_normalizes_email_and_trims_user_name()
    {
        var user = User.Register("  Jan.Kowalski@Example.PL ", " Borowik ", "hash", DateTimeOffset.UnixEpoch);

        Assert.Equal("jan.kowalski@example.pl", user.Email);
        Assert.Equal("Borowik", user.UserName);
        Assert.Equal(UserRole.Member, user.Role);
    }
}
