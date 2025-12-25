using Core.Logic.Connections.RabbitMQ.Models;

namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQPublisher
{
    Task PublishAsync(MessageRequest message, PublishArguments arguments);
}
