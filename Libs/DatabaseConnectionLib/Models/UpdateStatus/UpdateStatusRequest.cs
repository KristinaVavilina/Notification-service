using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.UpdateStatus;

public record class UpdateStatusRequest
{
    public required Guid Id { get; init; }
    public required MessageStatus Status { get; init; }
}
