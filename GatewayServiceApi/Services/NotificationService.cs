using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.StoreMessage;
using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;

namespace GatewayServiceApi.Services;

public class NotificationService : INotificationService
{
    private readonly IDatabaseConnectionService _databaseService;
    private readonly IMessageQueueConnectionService _messageQueueService;

    public NotificationService(
        IDatabaseConnectionService databaseService,
        IMessageQueueConnectionService messageQueueService)
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
        var result = await _messageQueueService.SendNotificationAsync(new SendNotificationRequest
        {
            Id = id,
            ChannelType = dto.ChannelType,
            Content = dto.Content,
            Recipient = dto.Recipient,
            Subject = dto.Subject,
            Metadata = dto.Metadata
        });

        return id;
    }
}
