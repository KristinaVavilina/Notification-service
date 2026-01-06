using EmailNotificationApi.Interfaces;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace EmailNotificationApi.Listeners.RabbitMQ;

public class NotificationRabbitMQListener : BackgroundService
{
    private IMessageQueueConnectionService _messageQueueConnectionService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public NotificationRabbitMQListener(
        IMessageQueueConnectionService messageQueueConnectionService,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _messageQueueConnectionService = messageQueueConnectionService;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    private async Task HandleNotificationAsync(MessageDto messageDto)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                Console.WriteLine($"Отправка письма для: {messageDto.Recipient}...");

                await emailService.SendEmailAsync(
                    messageDto.Recipient,
                    messageDto.Subject ?? "Уведомление",
                    messageDto.Content
                );

                Console.WriteLine("Письмо успешно отправлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки: {ex.Message}");
            }
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var channel = _configuration.GetValue<string>("ChannelName") ?? "default_queue"; ;
        _messageQueueConnectionService.Subscribe(channel, HandleNotificationAsync);

        return Task.CompletedTask;
    }
}
