using Dal.Message.Interfaces;
using Logic.Message.Interfaces;
using Logic.Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Message.Models;
using Core;
using Core.Exceptions;


namespace Logic.Message;

internal class MessageLogicManager : IMessageLogicManager
{
    private readonly IMessageRepository _messageRepository;

    public MessageLogicManager(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Guid> CreateAsync(MessageLogic logic)
    {
        await _messageRepository.CreateAsync(new MessageDal
        {
            Id = logic.Id,
            Message = logic.Message,
            Channel = logic.Channel,
            Recipient = logic.Recipient,
            Subject = logic.Subject,
            Metadata = logic.Metadata
            .Select(kv => new MessageMetadataDal
            {
                MessageId = logic.Id,
                Key = kv.Key,
                Value = kv.Value
            })
            .ToList()
        });
        return logic.Id;
    }

    public async Task UpdateStatusAsync(MessageStatusLogic logic)
    {
        var messageId = await _messageRepository.AddStatusAsync(new MessageStatusDal
        {
            MessageId = logic.MessageId,
            Status = logic.Status,
            TimeStamp = DateTime.UtcNow
        });

        if (messageId is null)
        {
            throw new DataNotFoundException($"Сообщение {messageId} не найдено");
        }
    }

    public async Task<MessageLogic> GetContent(Guid id)
    {
        var dal = await GetRequiredMessageAsync(id);
        return new MessageLogic
        {
            Message = dal.Message,
            Channel = dal.Channel,
            Metadata = dal.Metadata.ToDictionary(it => it.Key, it => it.Value),
            Id = dal.Id,
            Recipient = dal.Recipient,
            Subject = dal.Subject
        };
    }

    public async Task<MessageStatus> GetCurrentStatusAsync(Guid id)
    {
        var dal = await GetRequiredMessageAsync(id);
        var latest = dal.StatusHistory.AsQueryable().MaxBy(it => it.TimeStamp);
        if (latest is null)
        {
            throw new Exception();
        }
        return latest.Status;
    }

    public async Task<StatusHistoryLogic> GetStatusHistoryAsync(Guid id)
    {
        var dal = await GetRequiredMessageAsync(id);
        var result = dal.StatusHistory.Select(it => new StatusHistoryLogic.Entry
        {
            Status = it.Status,
            TimeStamp = it.TimeStamp,
        }).ToArray();
        return new StatusHistoryLogic
        {
            Entries = result
        };
    }

    private async Task<MessageDal> GetRequiredMessageAsync(Guid id)
    {
        var dal = await _messageRepository.GetAsync(id);
        if (dal is null)
        {
            throw new DataNotFoundException($"Сообщение {id} не найдено");
        }
        return dal;
    }
}
