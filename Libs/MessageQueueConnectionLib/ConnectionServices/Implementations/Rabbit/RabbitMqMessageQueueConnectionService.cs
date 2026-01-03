using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace MessageQueueConnectionLib.ConnectionServices.Implementations;

public class RabbitMqMessageQueueConnectionService : IMessageQueueConnectionService
{
    private readonly IRabbitMQPublisher _publisher;
    private readonly IRabbitMQListener _listener;
    private readonly IRabbitMQConnectionFactory _connectionFactory;

    public RabbitMqMessageQueueConnectionService(
        IRabbitMQPublisher publisher,
        IRabbitMQListener listener,
        IRabbitMQConnectionFactory connectionFactory)
    {
        _publisher = publisher;
        _listener = listener;
        _connectionFactory = connectionFactory;
    }

    public async Task SendNotificationAsync(MessageDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Recipient))
            throw new ArgumentException("Получатель не указан");

        var exchangeName = "notifications.direct";
        var routingKey = request.ChannelType.ToLower();

        var arguments = new PublishArguments
        {
            ExchangeName = exchangeName,
            RoutingKey = routingKey,
            Properties = null
        };

        Console.WriteLine($"[Gateway] Отправка сообщения {request.Id} в канал {routingKey}");

        try
        {
            await _publisher.PublishAsync(request, arguments);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Gateway] Ошибка отправки {request.Id}: {ex.Message}");
            throw;
        }
    }

    public void Subscribe(string queueName, Func<MessageDto, Task> onMessageReceived)
    {
        _listener.Subscribe<MessageDto>(queueName, onMessageReceived);

        Console.WriteLine($"[Service] Подписка на очередь {queueName} активирована.");
    }

    public bool IsConnected()
    {
        try
        {
            var connection = _connectionFactory.GetConnection();
            return connection.IsOpen;
        }
        catch
        {
            return false;
        }
    }
}