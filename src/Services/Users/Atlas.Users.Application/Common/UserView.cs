using Atlas.Users.Domain;

namespace Atlas.Users.Application.Common;

public sealed record UserView(Guid Id, string Email, string UserName, string Role, DateTimeOffset CreatedAt)
{
    public static UserView From(User user) =>
        new(user.Id, user.Email, user.UserName, user.Role.ToString(), user.CreatedAt);
}
