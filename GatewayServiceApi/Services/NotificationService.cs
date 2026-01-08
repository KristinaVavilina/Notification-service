using Core;
using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.StoreMessage;
using DatabaseConnectionLib.Models.UpdateStatus;
using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace GatewayServiceApi.Services;

public class NotificationService : INotificationService
{
    private const string RetryCountKey = "RetryCount";
    private const string TimeOutKey = "RetryTimeout_sec";

    private readonly IDatabaseConnectionService _databaseService;
    private readonly IMessageQueueConnectionService _messageQueueService;
    private readonly AsyncRetryPolicy<SendNotificationResponse> _retryPolicy;

    public NotificationService(
        IDatabaseConnectionService databaseService,
        IMessageQueueConnectionService messageQueueService,
        IConfiguration configuration)
    {
        _databaseService = databaseService;
        _messageQueueService = messageQueueService;

        var retryCount = configuration.GetSection(RetryCountKey).Get<int>();
        var timeout = TimeSpan.FromSeconds(configuration.GetSection(TimeOutKey).Get<float>());

        _retryPolicy = Policy<SendNotificationResponse>
            .Handle<Exception>()
            .OrResult(r => !r.IsSuccess)
            .WaitAndRetryAsync(
        retryCount: retryCount,
        sleepDurationProvider: retryAttempt =>
            timeout,
        onRetry: async (result, timespan, retry, context) =>
        {
            // логирование
            await _databaseService.UpdateStatusAsync(new UpdateStatusRequest
            {
                Id = result.Result.MessageId,
                Status = MessageStatus.Failed
            });
        });
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

        var request = new SendNotificationRequest
        {
            Id = id,
            ChannelType = dto.ChannelType,
            Content = dto.Content,
            Recipient = dto.Recipient,
            Subject = dto.Subject,
            Metadata = dto.Metadata
        };

        var response = await _retryPolicy.ExecuteAsync(() =>
            _messageQueueService.SendNotificationAsync(request));

        if (response.IsSuccess)
        {
            await UpdateStatus(id, MessageStatus.Sent);
        }
        else
        {
            await UpdateStatus(id, MessageStatus.Failed);
        }

        return id;
    }

    private async Task UpdateStatus(Guid id, MessageStatus status)
    {
        await _databaseService.UpdateStatusAsync(new UpdateStatusRequest
        {
            Id = id,
            Status = status
        });
    }
}
