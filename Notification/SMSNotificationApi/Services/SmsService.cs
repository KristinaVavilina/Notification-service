using SMSNotificationApi.Interfaces;

namespace EmailNotificationApi.Services;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmsService> _logger;

    public SmsService(IConfiguration configuration, ILogger<SmsService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var phone = _configuration.GetValue<string>("Sms:Phone");

        _logger.LogInformation($"Отправка смс \"{message}\" по номеру {phoneNumber}");
        _logger.LogInformation($"Смс отправлено. Отправитель: {phone}");
    }
}