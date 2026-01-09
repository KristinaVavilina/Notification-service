using PushNotificationApi.Interfaces;

namespace PushNotificationApi.Services;

public class PushService : IPushService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PushService> _logger;

    public PushService(IConfiguration configuration, ILogger<PushService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendPushAsync(string deviceToken, string title, string message)
    {
        var fcmUrl = _configuration.GetValue<string>("Push:FirebaseUrl");

        _logger.LogInformation($"Попытка отправки Push на токен {deviceToken}...");

        await Task.Delay(1000);
        throw new Exception("Тестовая ошибка отправки Push-уведомления.");
    }
}