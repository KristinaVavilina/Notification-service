using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace MessageQueueConnectionLib.ConnectionServices.Implementations;

public class RabbitMqMessageQueueConnectionService : IMessageQueueConnectionService
{
    private readonly IRabbitMQPublisher _publisher;
    private readonly IRabbitMQListener _listener;

    public RabbitMqMessageQueueConnectionService(IRabbitMQPublisher publisher, IRabbitMQListener listener)
    {
        _publisher = publisher;
        _listener = listener;
    }

    public async Task<SendNotificationResponse> SendNotificationAsync(SendNotificationRequest request)
    {
        var exchangeName = "notifications.direct";
        var routingKey = request.ChannelType.ToLower();

        var arguments = new PublishArguments
        {
            ExchangeName = exchangeName,
            RoutingKey = routingKey,
            Properties = null
        };

        try
        {
            return await _publisher.SendAsync<SendNotificationRequest, SendNotificationResponse>(request, arguments, CancellationToken.None);
        }
        catch (Exception ex)
        {
            return SendNotificationResponse.Failure(ex.Message, request.Id);
        }
    }

    public void Subscribe<TRequest, TResponse>(string queueName, Func<TRequest, Task<TResponse>> handler)
    {
        _listener.StartListening(queueName, handler);
    }
}