using Core;
using Core.Exceptions;
using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.StoreMessage;
using DatabaseConnectionLib.Models.UpdateStatus;
using GatewayServiceApi.Filters;
using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using MessageQueueConnectionLib.ConnectionServices.Interfaces;
using Microsoft.Extensions.Configuration;
using MonitoringConnectionLib.Interfaces;
using Polly;
using Polly.Retry;

namespace GatewayServiceApi.Services;

public class NotificationService : INotificationService
{
    private const string RetryCountKey = "RetryCount";
    private const string TimeOutKey = "RetryTimeout_sec";
    private const string ValidChannelsKey = "ValidChannels";

    private readonly ILogger<NotificationService> _logger;
    private readonly IDatabaseConnectionService _databaseService;
    private readonly IMessageQueueConnectionService _messageQueueService;
    private readonly AsyncRetryPolicy<SendNotificationResponse> _retryPolicy;
    private readonly HashSet<string> _validChannels;
    private readonly ICounter _sentNotifications;
    private readonly ICounter _failedNotifications;

    public NotificationService(
        IDatabaseConnectionService databaseService,
        IMessageQueueConnectionService messageQueueService,
        IConfiguration configuration,
        ILogger<NotificationService> logger,
        IMetricsRegistry metrics)
    {
        _logger = logger;
        _databaseService = databaseService;
        _messageQueueService = messageQueueService;

        var retryCount = configuration.GetSection(RetryCountKey).Get<int>();
        var timeout = TimeSpan.FromSeconds(configuration.GetSection(TimeOutKey).Get<float>());
        _validChannels = configuration.GetSection(ValidChannelsKey).Get<string[]>().ToHashSet();

        _retryPolicy = Policy<SendNotificationResponse>
            .Handle<Exception>()
            .OrResult(r => !r.IsSuccess)
            .WaitAndRetryAsync(
        retryCount: retryCount,
        sleepDurationProvider: retryAttempt =>
            timeout,
        onRetry: async (result, timespan, retry, context) =>
        {
            _logger.LogInformation($"Retrying sending message {result.Result.MessageId}. Error message: {result.Result.ErrorMessage}");
            await _databaseService.UpdateStatusAsync(new UpdateStatusRequest
            {
                Id = result.Result.MessageId,
                Status = MessageStatus.Retrying
            });
        });

        _sentNotifications = metrics.GetOrCreateCounter("notifications_sent", "Successfuly sent notifications");
        _failedNotifications = metrics.GetOrCreateCounter("notifications_failed", "Successfuly failed notifications");
    }

    public async Task<Guid> PublishMessageAsync(NotificationDto dto)
    {
        if (!_validChannels.Contains(dto.ChannelType))
        {
            throw new InvalidChannelException($"{dto.ChannelType} is not valid Channel type");
        }

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

        var response = await _retryPolicy.ExecuteAsync(async () =>
        {
            _logger.LogInformation($"Queueing Message Id {request.Id}");
            return await _messageQueueService.SendNotificationAsync(request);
        });

        if (response.IsSuccess)
        {
            _sentNotifications.Inc(1);
            _logger.LogInformation($"Message Id {request.Id} sent successfuly");
            await UpdateStatus(id, MessageStatus.Sent);
        }
        else
        {
            _failedNotifications.Inc(1);
            _logger.LogInformation($"Message Id {request.Id} sending failed. Error message: {response.ErrorMessage}");
            await UpdateStatus(id, MessageStatus.Failed);
            throw new NotificationServiceFailedException(response.ErrorMessage);
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
