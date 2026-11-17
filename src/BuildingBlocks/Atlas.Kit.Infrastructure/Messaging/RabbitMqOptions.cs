namespace Atlas.Kit.Infrastructure.Messaging;

/// <summary>Połączenie z RabbitMQ (sekcja "RabbitMq" w konfiguracji).</summary>
public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; init; } = "localhost";

    public string VirtualHost { get; init; } = "/";

    public string Username { get; init; } = "guest";

    public string Password { get; init; } = "guest";
}
