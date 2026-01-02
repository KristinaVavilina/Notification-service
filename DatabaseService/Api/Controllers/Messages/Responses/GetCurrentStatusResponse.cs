using Core;

namespace Api.Controllers.Messages.Responses;

public class GetCurrentStatusResponse
{
    public required MessageStatus Status { get; init; }
}
