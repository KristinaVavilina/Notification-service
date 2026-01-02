using MessageQueueConnectionLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueConnectionLib;

public static class MessageQueueConnectionLibStartUp
{
    public static IServiceCollection AddMessageQueueConnectionLib(this IServiceCollection services)
    {
        //services.AddSingleton<IMessageQueueConnectionService, RabbitMqConnectionService>();
        return services;
    }
}
