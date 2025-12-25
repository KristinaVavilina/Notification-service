using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Services;

namespace GatewayServiceApi;

public static class ApiStartUp
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<INotificationService, NotificationService>();
        return services;
    }
}
