namespace MessageQueueConnectionLib.Interfaces;

public interface IRabbitMqRequestService
{
    Task PublishAsync(string queue, MessageRequest message);
}
