using Atlas.Kit.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Forum.Application;

public static class ForumApplicationRegistration
{
    public static IServiceCollection AddForumApplication(this IServiceCollection services) =>
        services.AddKitApplication(typeof(ForumApplicationRegistration).Assembly);
}
