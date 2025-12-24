namespace MessageQueueConnectionLib.Interfaces;

public interface IMessageQueueConnectionService
{
    void Subscribe<T>(string queue, Func<T, Task> handler);
}
