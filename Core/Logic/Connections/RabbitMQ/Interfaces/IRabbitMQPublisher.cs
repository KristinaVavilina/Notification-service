using Core.Logic.Connections.RabbitMQ.Models;
using RabbitMQ.Client;

namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQPublisher
{
    void Publish<T>(T message, IModel channel, PublishArguments arguments);

    Task<TResponse?> SendAsync<TRequest, TResponse>(TRequest request, PublishArguments arguments, CancellationToken cancellationToken);
}
