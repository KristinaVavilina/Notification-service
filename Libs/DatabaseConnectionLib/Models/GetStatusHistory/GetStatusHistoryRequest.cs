using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.GetStatusHistory;

public record class GetStatusHistoryRequest
{
    public required Guid Id { get; init; }
}
