using PushNotificationApi.Interfaces;

namespace PushNotificationApi.Services;

public class PushService : IPushService
{
    private readonly IConfiguration _configuration;

    public PushService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPushAsync(string deviceToken, string title, string message)
    {
        var fcmUrl = _configuration.GetValue<string>("Push:FirebaseUrl");

        Console.WriteLine($"Попытка отправки Push на токен {deviceToken}...");

        await Task.Delay(1000);
        throw new Exception("Тестовая ошибка отправки Push-уведомления.");
    }
}