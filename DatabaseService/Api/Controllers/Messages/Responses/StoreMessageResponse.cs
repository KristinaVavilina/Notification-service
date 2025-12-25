namespace Api.Controllers.Messages.Responses;

public record class StoreMessageResponse
{
    public required Guid MessageId { get; init; }
}
