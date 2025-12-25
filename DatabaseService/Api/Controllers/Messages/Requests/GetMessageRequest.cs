namespace Api.Controllers.Messages.Requests;

public record class GetMessageRequest
{
    public required Guid Id { get; init; }
}
