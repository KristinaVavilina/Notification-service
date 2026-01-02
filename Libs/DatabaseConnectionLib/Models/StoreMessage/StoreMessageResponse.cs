using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.StoreMessage;

public record class StoreMessageResponse
{
    public required Guid MessageId { get; init; }
}
