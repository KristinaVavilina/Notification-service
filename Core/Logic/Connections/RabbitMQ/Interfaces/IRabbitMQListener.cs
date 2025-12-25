namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQListener
{
    void Subscribe(string queue, Func<MessageRequest, Task> handler);
}
