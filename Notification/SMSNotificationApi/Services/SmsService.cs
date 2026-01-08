using SMSNotificationApi.Interfaces;

namespace EmailNotificationApi.Services;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var phone = _configuration.GetValue<string>("Sms:Phone");

        Console.WriteLine($"Отправка смс \"{message}\" по номеру {phoneNumber}");
        Console.WriteLine($"Смс отправлено. Отправитель: {phone}");
    }
}