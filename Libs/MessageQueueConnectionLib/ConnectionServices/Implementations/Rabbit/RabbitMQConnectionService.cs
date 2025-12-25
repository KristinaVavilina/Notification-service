using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace MessageQueueConnectionLib.ConnectionServices.Implementations;

public class ConnectionService : IConnectionService
{
    private readonly IRabbitMQPublisher _publisher;
    private readonly IRabbitMQConnectionFactory _connectionFactory;

    public ConnectionService(IRabbitMQPublisher publisher, IRabbitMQConnectionFactory connectionFactory)
    {
        _publisher = publisher;
        _connectionFactory = connectionFactory;
    }

    public async Task SendNotificationAsync(MessageRequest request)
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