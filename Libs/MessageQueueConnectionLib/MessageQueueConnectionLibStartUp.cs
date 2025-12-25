using Core.Logic.Connections.RabbitMQ;
using Core.Logic.Connections.RabbitMQ.Interfaces;
using MessageQueueConnectionLib.ConnectionServices.Implementations;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueueConnectionLib;

public static class MessageQueueConnectionLibStartUp
{
    public static IServiceCollection AddMessageQueueConnectionLib(this IServiceCollection services, string host, string user, string pass)
    {
        services.AddSingleton<IRabbitMQConnectionFactory>(new RabbitMQConnectionFactory(host, user, pass));

        services.AddSingleton<IRabbitMQPublisher, RabbitMqPublisher>();
        services.AddSingleton<IRabbitMQListener, RabbitMqListener>();

        services.AddScoped<IConnectionService, ConnectionService>();

        return services;
    }
}
