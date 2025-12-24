using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.GetCurrentStatus;

public record class GetCurrentStatusRequest
{
    public required Guid Id { get; init; }
}
