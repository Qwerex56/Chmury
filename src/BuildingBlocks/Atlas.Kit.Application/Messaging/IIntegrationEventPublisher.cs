namespace Atlas.Kit.Application.Messaging;

/// <summary>Port do publikowania zdarzeń integracyjnych (RabbitMQ przez MassTransit w Infrastructure).</summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : class;
}
