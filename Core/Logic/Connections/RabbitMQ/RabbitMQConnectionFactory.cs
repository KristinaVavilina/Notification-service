using RabbitMQ.Client;
using Core.Logic.Connections.RabbitMQ.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Logic.Connections.RabbitMQ;

public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
    private readonly IConnectionFactory _factory;
    private IConnection? _connection;
    private readonly IConfiguration _configuration;

    public RabbitMQConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;

        _factory = new ConnectionFactory
        {
            HostName = _configuration.GetValue<string>("RabbitMq:HostName"),
            UserName = _configuration.GetValue<string>("RabbitMq:UserName"),
            Password = _configuration.GetValue<string>("RabbitMq:Password"),

            Port = _configuration.GetValue<int>("RabbitMq:Port"),

            VirtualHost = _configuration.GetValue<string>("RabbitMq:VirtualHost") ?? "/",

            DispatchConsumersAsync = true
        };
    }

    public IConnection GetConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _factory.CreateConnection();
        }

        return _connection;
    }
}