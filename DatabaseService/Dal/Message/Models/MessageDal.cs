using Core;
using Core.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Message.Models;

public class MessageDal : BaseEntityDal<Guid>
{

    public string Channel { get; set; }

    public string Recipient { get; set; } = null!;

    public string? Subject { get; set; }

    public string Message { get; set; } = null!;

    public ICollection<MessageMetadataDal> Metadata { get; set; }
    = new List<MessageMetadataDal>();

    public ICollection<MessageStatusDal> StatusHistory { get; set; }
    = new List<MessageStatusDal>();
}
