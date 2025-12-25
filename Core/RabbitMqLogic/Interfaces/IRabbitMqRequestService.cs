namespace Core.RabbitMqLogic.Interfaces;

public interface IRabbitMqRequestService
{
    Task PublishAsync<T>(string queue, T message);
}
