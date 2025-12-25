namespace Api.Controllers.Messages.Responses;

public record class SetMessageStatusResponse
{
    public required bool Success { get; init; }
}
