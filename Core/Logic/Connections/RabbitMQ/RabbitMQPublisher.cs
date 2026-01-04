using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using Core.Logic.Serialization.Interfaces;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace Core.Logic.Connections.RabbitMQ;

public class RabbitMqPublisher : IRabbitMQPublisher
{
    private readonly IRabbitMQConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _serializer;

    public RabbitMqPublisher(IRabbitMQConnectionFactory connectionFactory, IMessageSerializer serializer)
    {
        _connectionFactory = connectionFactory;
        _serializer = serializer;
    }

    public Task PublishAsync<T>(T message, PublishArguments arguments)
    {
        var connection = _connectionFactory.GetConnection();
        using var channel = connection.CreateModel();

        var body = _serializer.Serialize(message);

        var props = arguments.Properties ?? channel.CreateBasicProperties();
        props.Persistent = true;

        channel.ExchangeDeclare(exchange: arguments.ExchangeName, type: ExchangeType.Direct);
        channel.QueueDeclare(queue: arguments.RoutingKey, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(queue: arguments.RoutingKey, exchange: arguments.ExchangeName, routingKey: arguments.RoutingKey);

        channel.BasicPublish(
            exchange: arguments.ExchangeName,
            routingKey: arguments.RoutingKey,
            basicProperties: props,
            body: body);

        return Task.CompletedTask;
    }
}
