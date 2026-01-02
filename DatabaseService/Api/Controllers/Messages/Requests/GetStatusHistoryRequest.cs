namespace Api.Controllers.Messages.Requests;

public record class GetStatusHistoryRequest
{
    public required Guid Id { get; init; }
}
