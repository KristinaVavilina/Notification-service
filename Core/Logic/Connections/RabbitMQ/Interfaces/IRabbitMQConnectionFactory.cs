using RabbitMQ.Client;

namespace Core.Logic.Connections.RabbitMQ.Interfaces;

public interface IRabbitMQConnectionFactory
{
    IConnection GetConnection();
}