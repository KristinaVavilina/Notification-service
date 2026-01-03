using RabbitMQ.Client;

namespace Core.Logic.Connections.RabbitMQ.Models;

public record PublishArguments
{
    public required IBasicProperties? Properties { get; init; }

    public required string ExchangeName { get; init; }

    public required string RoutingKey { get; init; }
}