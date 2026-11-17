namespace Atlas.Users.Api.Endpoints;

public sealed record RegisterRequest(string Email, string UserName, string Password);
