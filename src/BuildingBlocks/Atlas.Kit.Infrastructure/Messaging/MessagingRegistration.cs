using Atlas.Kit.Application.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Kit.Infrastructure.Messaging;

public static class MessagingRegistration
{
    /// <summary>
    /// Konfiguruje MassTransit na RabbitMQ. <paramref name="serviceName"/> jest prefiksem nazw kolejek,
    /// dzięki czemu każdy serwis ma własne kolejki dla tych samych zdarzeń.
    /// </summary>
    public static IServiceCollection AddKitMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName,
        Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        var options = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>() ?? new RabbitMqOptions();

        services.AddMassTransit(bus =>
        {
            bus.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(serviceName, includeNamespace: false));
            configureConsumers?.Invoke(bus);

            bus.UsingRabbitMq((context, rabbit) =>
            {
                rabbit.Host(options.Host, options.VirtualHost, host =>
                {
                    host.Username(options.Username);
                    host.Password(options.Password);
                });
                rabbit.UseMessageRetry(retry => retry.Intervals(500, 2000, 5000));
                rabbit.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IIntegrationEventPublisher, MassTransitEventPublisher>();
        return services;
    }
}
