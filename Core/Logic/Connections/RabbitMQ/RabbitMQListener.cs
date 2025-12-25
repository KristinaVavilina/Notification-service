using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Serialization.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqListener : IRabbitMQListener, IDisposable
{
    private readonly IRabbitMQConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _serializer;
    private IModel? _channel;

    public RabbitMqListener(IRabbitMQConnectionFactory connectionFactory, IMessageSerializer serializer)
    {
        _connectionFactory = connectionFactory;
        _serializer = serializer;
    }

    public void Subscribe(string queue, Func<MessageRequest, Task> handler)
    {
        var connection = _connectionFactory.GetConnection();

        _channel = connection.CreateModel();
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = _serializer.Deserialize<MessageRequest>(body);

            if (message != null)
            {
                try
                {
                    await handler(message);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки: {ex.Message}");

                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            }
            else
            {
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
        };

        _channel.BasicConsume(queue, autoAck: false, consumer);
    }

    public void Dispose() // Нужен?
    {
        _channel?.Close();
        _channel?.Dispose();
    }
}