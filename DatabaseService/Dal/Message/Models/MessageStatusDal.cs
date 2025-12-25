using Core;
using Core.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Message.Models;

public record class MessageStatusDal
{
    public required Guid MessageId { get; set; }
    public MessageDal Message { get; set; } = null!;
    public required MessageStatus Status { get; set; }
    public required DateTime TimeStamp { get; set; }
}
