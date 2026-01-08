using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using PushNotificationApi.Interfaces;

namespace PushNotificationApi.Listeners.RabbitMQ;

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

    private async Task<SendNotificationResponse> HandleNotificationAsync(SendNotificationRequest messageDto)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var pushService = scope.ServiceProvider.GetRequiredService<IPushService>();

            Console.WriteLine($"Отправка push для: {messageDto.Recipient}...");

            try
            {
                await pushService.SendPushAsync(
                    messageDto.Recipient,
                    messageDto.Subject ?? "Push",
                    messageDto.Content
                );

                Console.WriteLine("Push успешно отправлен!");
                return SendNotificationResponse.Success(messageDto.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки: {ex.Message}");
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
