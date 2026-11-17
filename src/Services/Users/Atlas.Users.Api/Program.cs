using Atlas.Kit.Api;
using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using Atlas.Users.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddKitApi(typeof(Program).Assembly);
builder.Services.AddUsersApplication();
builder.Services.AddUsersInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseKitApi("users");
await app.MigrateDatabaseAsync<UsersDbContext>();
await app.RunAsync();
