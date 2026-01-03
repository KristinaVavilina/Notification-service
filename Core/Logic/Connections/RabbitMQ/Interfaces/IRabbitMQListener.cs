namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQListener
{
    void Subscribe<T>(string queue, Func<T, Task> handler);
}
