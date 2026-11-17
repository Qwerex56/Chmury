using Atlas.Kit.Infrastructure.Messaging;
using Atlas.Kit.Infrastructure.Persistence;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Infrastructure.Persistence;
using Atlas.Users.Infrastructure.Repositories;
using Atlas.Users.Infrastructure.Security;
using Atlas.Users.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Infrastructure;

public static class UsersInfrastructureRegistration
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Users")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
        services.AddSingleton<ITokenIssuer, JwtTokenIssuer>();
        services.AddScoped<IDataSeeder, DemoUserSeeder>();
        services.AddSingleton(TimeProvider.System);

        services.AddKitMessaging(configuration, "users");
        return services;
    }
}
