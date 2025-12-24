using GatewayServiceApi.Models;

namespace GatewayServiceApi.Interfaces;

public interface INotificationService
{
    public Task<Guid> PublishMessageAsync(NotificationDto dto);
}
