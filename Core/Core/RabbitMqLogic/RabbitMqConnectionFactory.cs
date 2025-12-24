using RabbitMQ.Client;

namespace MessageQueueConnectionLib.Rabbit;

public class RabbitMqConnectionFactory
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;

    public RabbitMqConnectionFactory(string host, string user, string pass)
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
