using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Models.GetContent;

public record class GetContentResponse
{
    public required Guid Id { get; set; }

    public required string Channel { get; set; }

    public required string Recipient { get; set; } = null!;

    public required string? Subject { get; set; }

    public required string Message { get; set; } = null!;

    public required Dictionary<string, string> Metadata { get; set; }
}
