namespace MessageQueueConnectionLib.ConnectionServices.Interfaces;

public interface IMessageQueueConnectionService
{
    Task<SendNotificationResponse> SendNotificationAsync(SendNotificationRequest request);

    void Subscribe<TRequest, TResponse>(string queueName, Func<TRequest, Task<TResponse>> handler);
}