using Atlas.Kit.Api;
using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Map.Application;
using Atlas.Map.Infrastructure;
using Atlas.Map.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddKitApi(typeof(Program).Assembly);
builder.Services.AddMapApplication();
builder.Services.AddMapInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseKitApi("map");
await app.MigrateDatabaseAsync<MapDbContext>();
await app.RunAsync();
