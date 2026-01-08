namespace PushNotificationApi.Interfaces;

public interface IPushService
{
    Task SendPushAsync(string deviceToken, string title, string message);
}
