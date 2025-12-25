using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.StoreMessage;
using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using MessageQueueConnectionLib;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace GatewayServiceApi.Services;

public class NotificationService : INotificationService
{
    private readonly IDatabaseConnectionService _databaseService;
    private readonly IConnectionService _messageQueueService;

    public NotificationService(
        IDatabaseConnectionService databaseService,
        IConnectionService messageQueueService)
    {
        _databaseService = databaseService;
        _messageQueueService = messageQueueService;
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
        _messageQueueService.SendNotificationAsync(new MessageRequest
        {
            Id = id,
            ChannelType = dto.ChannelType,
            Content = dto.Content,
            Recipient = dto.Recipient,
            Subject = dto.Subject,
            CreatedAt = DateTime.UtcNow
        });
        return id;
    }
}
