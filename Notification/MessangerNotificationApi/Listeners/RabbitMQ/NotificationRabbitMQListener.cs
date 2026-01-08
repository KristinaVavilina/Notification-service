using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using MessangerNotificationApi.Interfaces;

namespace MessangerNotificationApi.Listeners.RabbitMQ;

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
            var messangerService = scope.ServiceProvider.GetRequiredService<IMessangerService>();

            Console.WriteLine($"Отправка сообщения для: {messageDto.Recipient}...");

            try
            {
                await messangerService.SendMessageAsync(
                    messageDto.Recipient,
                    messageDto.Content
                );

                Console.WriteLine("Сообщение успешно отправлено!");
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
