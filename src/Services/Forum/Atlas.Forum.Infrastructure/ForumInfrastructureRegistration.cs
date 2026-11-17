using Atlas.Forum.Application.Abstractions;
using Atlas.Forum.Infrastructure.Consumers;
using Atlas.Forum.Infrastructure.Persistence;
using Atlas.Forum.Infrastructure.Readers;
using Atlas.Forum.Infrastructure.Repositories;
using Atlas.Forum.Infrastructure.Seeding;
using Atlas.Kit.Infrastructure.Messaging;
using Atlas.Kit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Forum.Infrastructure;

public static class ForumInfrastructureRegistration
{
    public static IServiceCollection AddForumInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ForumDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Forum")));

        services.AddScoped<IForumThreadRepository, ForumThreadRepository>();
        services.AddScoped<IForumReader, ForumReader>();
        services.AddScoped<IDataSeeder, DemoForumSeeder>();
        services.AddSingleton(TimeProvider.System);

        services.AddKitMessaging(configuration, "forum", bus => bus.AddConsumer<FindingReportedConsumer>());
        return services;
    }
}
