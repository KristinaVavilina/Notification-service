using EmailNotificationApi.Interfaces;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace EmailNotificationApi.Listeners.RabbitMQ;

public class NotificationRabbitMQListener : BackgroundService
{
    private IMessageQueueConnectionService _messageQueueConnectionService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NotificationRabbitMQListener> _logger;

    public NotificationRabbitMQListener(
        IMessageQueueConnectionService messageQueueConnectionService,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<NotificationRabbitMQListener> logger)
    {
        _messageQueueConnectionService = messageQueueConnectionService;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    private async Task<SendNotificationResponse> HandleNotificationAsync(SendNotificationRequest messageDto)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            _logger.LogInformation($"Отправка письма для: {messageDto.Recipient}...");

            try
            {
                await emailService.SendEmailAsync(
                    messageDto.Recipient,
                    messageDto.Subject ?? "Уведомление",
                    messageDto.Content
                );

                _logger.LogInformation("Письмо успешно отправлено!");
                return SendNotificationResponse.Success(messageDto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Ошибка отправки: {ex.Message}");
                return SendNotificationResponse.Failure(ex.Message, messageDto.Id);
            }
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _configuration.GetValue<string>("ChannelName") ?? "default_queue";
        stoppingToken.ThrowIfCancellationRequested();
        _messageQueueConnectionService.Subscribe<SendNotificationRequest, SendNotificationResponse>(channel, HandleNotificationAsync);

        return Task.CompletedTask;
    }
}
