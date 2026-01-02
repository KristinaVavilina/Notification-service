using Core;

namespace Api.Controllers.Messages.Requests;

public record class SetMessageStatusRequest
{
    public required Guid Id { get; init; }
    public required MessageStatus Status { get; init; }
}
