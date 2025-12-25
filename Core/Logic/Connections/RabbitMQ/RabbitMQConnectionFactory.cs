using RabbitMQ.Client;
using Core.Logic.Connections.RabbitMQ.Interfaces;

namespace Core.Logic.Connections.RabbitMQ;

public class RabbitMQConnectionFactory: IRabbitMQConnectionFactory
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;

    public RabbitMQConnectionFactory(string host, string user, string pass)
    {
        _factory = new ConnectionFactory
        {
            HostName = host,
            UserName = user,
            Password = pass,
            DispatchConsumersAsync = true
        };
    }

    public IConnection GetConnection()
    {
        if (_connection == null || !_connection.IsOpen)
            _connection = _factory.CreateConnection();

        return _connection;
    }
}
