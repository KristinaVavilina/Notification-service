using MessageQueueConnectionLib.ConnectionServices.Implementations;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueueConnectionLib;

public static class MessageQueueConnectionLibStartUp
{
    public static IServiceCollection AddMessageQueueConnectionLib(this IServiceCollection services)
    {
        services.AddScoped<IMessageQueueConnectionService, RabbitMqMessageQueueConnectionService>();

        return services;
    }
}