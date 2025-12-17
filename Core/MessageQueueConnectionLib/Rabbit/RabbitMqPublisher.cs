using System.Text;
using System.Text.Json;
using MessageQueueConnectionLib.Interfaces;
using RabbitMQ.Client;


namespace MessageQueueConnectionLib.Rabbit;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly RabbitMqConnectionFactory _connectionFactory;

    public RabbitMqPublisher(RabbitMqConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task PublishAsync<T>(string queue, T message)
    {
        var connection = _connectionFactory.GetConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = channel.CreateBasicProperties();
        props.Persistent = true;

        channel.BasicPublish("", queue, props, body);

        return Task.CompletedTask;
    }
}
