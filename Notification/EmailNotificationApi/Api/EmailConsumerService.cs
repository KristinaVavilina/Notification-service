using MessageQueueConnectionLib.Interfaces;

public class EmailConsumerService : BackgroundService
{
    private readonly IMessageConsumer _consumer;

    public EmailConsumerService(IMessageConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe<string>("email_queue", (message) =>
        {
            Console.WriteLine($"Получено сообщение: {message}");
            return Task.CompletedTask;
        });

        return Task.CompletedTask;
    }
}