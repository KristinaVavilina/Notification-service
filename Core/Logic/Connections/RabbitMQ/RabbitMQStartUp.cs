using Core.Logic.Connections.RabbitMQ.Generators.CorrelationId;
using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Serialization;
using Core.Logic.Serialization.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Logic.Connections.RabbitMQ;

public static class RabbitMQStartUp
{
    public static IServiceCollection AddRabbitMQServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        services.AddSingleton<IMessageSerializer, MessageSerializer>();

        services.AddSingleton<ICorrelationIdGenerator, CorrelationIdGenerator>();

        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
        services.AddSingleton<IRabbitMQListener, RabbitMQListener>();  

        return services;
    }
}