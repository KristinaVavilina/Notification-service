namespace MessageQueueConnectionLib.ConnectionServices.Interfaces;

public interface IConnectionService
{
    Task SendNotificationAsync(MessageRequest request);

    bool IsConnected();
}