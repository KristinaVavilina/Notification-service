using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Services.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib;

public static class DatabaseConnectionLibStartUp
{
    public static IServiceCollection AddDatabaseConnectionLib(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseConnectionService, HttpDatabaseConnectionService>();
        return services;
    }
}
