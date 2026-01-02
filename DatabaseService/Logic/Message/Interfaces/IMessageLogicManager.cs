using Core;
using Logic.Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Message.Interfaces;

public interface IMessageLogicManager
{
    public Task<Guid> CreateAsync(MessageLogic logic);

    public Task UpdateStatusAsync(MessageStatusLogic logic);

    public Task<MessageLogic> GetContent(Guid id);

    public Task<MessageStatus> GetCurrentStatusAsync(Guid id);

    public Task<StatusHistoryLogic> GetStatusHistoryAsync(Guid id);
}
