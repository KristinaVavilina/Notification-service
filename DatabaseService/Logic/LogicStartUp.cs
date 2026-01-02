using Logic.Message;
using Logic.Message.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class LogicStartUp
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<IMessageLogicManager, MessageLogicManager>();
            return services;
        }
    }
}
