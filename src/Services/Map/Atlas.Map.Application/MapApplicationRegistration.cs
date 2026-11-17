using Atlas.Kit.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Map.Application;

public static class MapApplicationRegistration
{
    public static IServiceCollection AddMapApplication(this IServiceCollection services) =>
        services.AddKitApplication(typeof(MapApplicationRegistration).Assembly);
}
