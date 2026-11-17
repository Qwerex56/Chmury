namespace Atlas.Users.Application.Common;

public sealed record AuthTokenView(string AccessToken, DateTimeOffset ExpiresAt, UserView User);
