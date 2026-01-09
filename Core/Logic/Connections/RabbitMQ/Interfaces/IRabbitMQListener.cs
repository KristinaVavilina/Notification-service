namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQListener
{
    public void StartListening<TRequest, TResponse>(string queueName, Func<TRequest, Task<TResponse>> handler);
}
