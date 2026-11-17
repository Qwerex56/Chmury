using Atlas.Kit.Infrastructure.Messaging;
using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Map.Application.Abstractions;
using Atlas.Map.Infrastructure.Persistence;
using Atlas.Map.Infrastructure.Readers;
using Atlas.Map.Infrastructure.Repositories;
using Atlas.Map.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Map.Infrastructure;

public static class MapInfrastructureRegistration
{
    public static IServiceCollection AddMapInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MapDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Map"), npgsql => npgsql.UseNetTopologySuite()));

        services.AddScoped<IFindingRepository, FindingRepository>();
        services.AddScoped<IMapReader, MapReader>();
        services.AddScoped<IDataSeeder, DemoMapSeeder>();
        services.AddSingleton(TimeProvider.System);

        services.AddKitMessaging(configuration, "map");
        return services;
    }
}
