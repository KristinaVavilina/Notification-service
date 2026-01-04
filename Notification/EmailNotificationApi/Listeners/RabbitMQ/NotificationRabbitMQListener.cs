
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace EmailNotificationApi.Listeners.RabbitMQ;

public class NotificationRabbitMQListener : BackgroundService
{
    private IMessageQueueConnectionService _messageQueueConnectionService;

    public NotificationRabbitMQListener(IMessageQueueConnectionService messageQueueConnectionService)
    {
        _messageQueueConnectionService = messageQueueConnectionService;
    }

    private async Task HandleNotificationAsync(MessageDto messageDto)
    {
        Console.WriteLine("!!!");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _messageQueueConnectionService.Subscribe("string", HandleNotificationAsync);

        return Task.CompletedTask;
    }
}
