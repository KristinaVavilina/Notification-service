using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Message.Models;

public class MessageLogic
{
    public Guid Id { get; set; }

    public string Channel { get; set; }

    public string Recipient { get; set; } = null!;

    public string? Subject { get; set; }

    public string Message { get; set; } = null!;

    public Dictionary<string, string> Metadata { get; set; }
}
