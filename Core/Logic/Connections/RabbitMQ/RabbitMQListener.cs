using Core.Logic.Connections.RabbitMQ.Generators.QueueName;
using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Serialization.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Logic.Connections.RabbitMQ;

internal class RabbitMQListener : IRabbitMQListener
{
    private readonly IRabbitMQConnectionFactory _connection;
    private readonly IRabbitMQPublisher _publisher;
    private readonly IMessageSerializer _serializer;
    private IModel _channel;

    public RabbitMQListener(IRabbitMQPublisher publisher,
        IRabbitMQConnectionFactory connection,
        IMessageSerializer serializer)
    {
        _publisher = publisher;
        _connection = connection;
        _serializer = serializer;
    }

    public void StartListening<TRequest, TResponse>(string queueName, Func<TRequest, Task<TResponse>> handler)
    {
        var connection = _connection.GetConnection();
        _channel = connection.CreateModel();
        var consumer = new AsyncEventingBasicConsumer(_channel);
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        consumer.Received += async (model, args) =>
        {
            try
            {
                await HandleRequestAsync(args, handler);
                _channel.BasicAck(args.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                _channel.BasicAck(args.DeliveryTag, multiple: false);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    private async Task HandleRequestAsync<TRequest, TResponse>(BasicDeliverEventArgs args, Func<TRequest, Task<TResponse>> handler)
    {
        var queueName = args.BasicProperties.ReplyTo;
        var request = _serializer.Deserialize<TRequest>(args.Body.ToArray());
        if (request is null)
        {
            throw new NotImplementedException();
        }
        var response = await handler(request);
        var properties = _channel.CreateBasicProperties();
        properties.CorrelationId = args.BasicProperties.CorrelationId;
        Console.WriteLine(args.BasicProperties.CorrelationId);
        _publisher.Publish(response, _channel, new Models.PublishArguments()
        {
            ExchangeName = "",
            RoutingKey = queueName,
            Properties = properties
        });
    }
}