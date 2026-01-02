using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Message.Models;

public record class StatusHistoryLogic
{
    public record class Entry
    {
        public required MessageStatus Status { get; init; }
        public required DateTime TimeStamp { get; init; }
    }

    public required Entry[] Entries { get; init; }
}
