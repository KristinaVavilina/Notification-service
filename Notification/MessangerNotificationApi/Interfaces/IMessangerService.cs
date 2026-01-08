namespace MessangerNotificationApi.Interfaces;

public interface IMessangerService
{
    Task SendMessageAsync(string chatId, string message);
}