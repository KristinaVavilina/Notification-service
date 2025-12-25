using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.GetStatusHistory;

public record class GetStatusHistoryResponse
{
    public record class Entry
    {
        public required MessageStatus Status { get; init; }
        public required DateTime TimeStamp { get; init; }
    }

    public required Entry[] Entries { get; init; }
}
