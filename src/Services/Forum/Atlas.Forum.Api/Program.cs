using Atlas.Kit.Api;
using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Forum.Application;
using Atlas.Forum.Infrastructure;
using Atlas.Forum.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddKitApi(typeof(Program).Assembly);
builder.Services.AddForumApplication();
builder.Services.AddForumInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseKitApi("forum");
await app.MigrateDatabaseAsync<ForumDbContext>();
await app.RunAsync();
