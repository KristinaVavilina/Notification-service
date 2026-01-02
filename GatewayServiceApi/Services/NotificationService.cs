using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.StoreMessage;
using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using MessageQueueConnectionLib.Interfaces;

namespace GatewayServiceApi.Services;

public class NotificationService : INotificationService
{
    private readonly IDatabaseConnectionService _databaseService;

    public NotificationService(
        IDatabaseConnectionService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<Guid> PublishMessageAsync(NotificationDto dto)
    {
        var id = Guid.NewGuid();
        await _databaseService.StoreMessageAsync(new StoreMessageRequest
        {
            Id = id,
            Channel = dto.ChannelType,
            Message = dto.Content,
            Subject = dto.Subject,
            Metadata = dto.Metadata,
            Recipient = dto.Recipient
        });
        return id;
    }
}
