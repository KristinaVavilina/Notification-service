using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Message.Models;

public record class MessageStatusLogic
{
    public required Guid MessageId { get; init; }
    public required MessageStatus Status { get; init; }
}
