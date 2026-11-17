using Atlas.Kit.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Users.Application;

public static class UsersApplicationRegistration
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services) =>
        services.AddKitApplication(typeof(UsersApplicationRegistration).Assembly);
}
