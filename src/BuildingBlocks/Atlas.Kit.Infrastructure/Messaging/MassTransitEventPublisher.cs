using Atlas.Kit.Application.Messaging;
using MassTransit;

namespace Atlas.Kit.Infrastructure.Messaging;

internal sealed class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IIntegrationEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : class =>
        publishEndpoint.Publish(integrationEvent, cancellationToken);
}
