using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.UpdateStatus;

public record class UpdateStatusResponse
{
    public required bool Success { get; init; }
}
