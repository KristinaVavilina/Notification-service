using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using Core.Logic.Serialization.Interfaces;
using RabbitMQ.Client;

namespace Core.Logic.Connections.RabbitMQ;

public class RabbitMqPublisher : IRabbitMQPublisher
{
    private readonly RabbitMQConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _serializer;

    public RabbitMqPublisher(RabbitMQConnectionFactory connectionFactory, IMessageSerializer serializer)
    {
        _connectionFactory = connectionFactory;
        _serializer = serializer;
    }

    public Task PublishAsync(MessageRequest message, PublishArguments arguments)
    {
        var connection = _connectionFactory.GetConnection();
        using var channel = connection.CreateModel();

        var body = _serializer.Serialize(message);

        var props = arguments.Properties ?? channel.CreateBasicProperties();
        props.Persistent = true;

        channel.BasicPublish(
            exchange: arguments.ExchangeName,
            routingKey: arguments.RoutingKey,
            basicProperties: props,
            body: body);

        return Task.CompletedTask;
    }
}
