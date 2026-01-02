namespace Api.Controllers.Messages.Requests;

public record class GetCurrentStatusRequest
{
    public required Guid Id { get; init; }
}
