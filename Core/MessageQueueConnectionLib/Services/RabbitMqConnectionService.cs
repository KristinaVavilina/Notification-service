using MessageQueueConnectionLib.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MessageQueueConnectionLib.Interfaces;

public class RabbitMqConnectionService: IMessageQueueConnectionService
{
    private readonly RabbitMqConnectionFactory _connectionFactory;

    public RabbitMqConnectionService(RabbitMqConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Subscribe<T>(string queue, Func<T, Task> handler)
    {
        var connection = _connectionFactory.GetConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(json);

            if (message != null)
            {
                try
                {
                    await handler(message);

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            }
            else
            {
                Console.WriteLine("Получено пустое сообщение");
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
        };

        channel.BasicConsume(queue, autoAck: false, consumer);
    }
}