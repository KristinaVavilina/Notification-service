namespace MessageQueueConnectionLib.ConnectionServices.Interfaces;

public interface IMessageQueueConnectionService
{
    Task SendNotificationAsync(MessageDto request);

    void Subscribe(string queueName, Func<MessageDto, Task> onMessageReceived);

    bool IsConnected();
}