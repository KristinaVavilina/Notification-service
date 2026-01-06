
using EmailNotificationApi.Interfaces;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace EmailNotificationApi.Listeners.RabbitMQ;

public class NotificationRabbitMQListener : BackgroundService
{
    private IMessageQueueConnectionService _messageQueueConnectionService;
    //private readonly IServiceProvider _serviceProvider; // Нужно для создания scope

    //public NotificationRabbitMQListener(
    //    IMessageQueueConnectionService messageQueueConnectionService,
    //    IServiceProvider serviceProvider)
    //{
    //    _messageQueueConnectionService = messageQueueConnectionService;
    //    _serviceProvider = serviceProvider;
    //}

    public NotificationRabbitMQListener(IMessageQueueConnectionService messageQueueConnectionService)
    {
        _messageQueueConnectionService = messageQueueConnectionService;
    }

    private async Task HandleNotificationAsync(MessageDto messageDto)
    {
        //// Создаем область видимости (Scope), так как BackgroundService - это Singleton,
        //// а EmailService может быть Scoped или Transient.
        //using (var scope = _serviceProvider.CreateScope())
        //{
        //    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        //    try
        //    {
        //        Console.WriteLine($"Отправка письма для: {"krvaviliii@gmail.com"}...");

        //        // Предполагаем, что в MessageDto есть поля Email, Subject и Content
        //        await emailService.SendEmailAsync(
        //            "krvaviliii@gmail.com",
        //            "Уведомление",
        //            $"Привет! Это сообщение из RabbitMQ: {messageDto.Content}"
        //        );

        //        Console.WriteLine("Письмо успешно отправлено!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Ошибка отправки: {ex.Message}");
        //        // Здесь можно добавить логику повторной отправки или логирование ошибки
        //    }
        //}
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        _messageQueueConnectionService.Subscribe("string", HandleNotificationAsync);

        return Task.CompletedTask;
    }
}
