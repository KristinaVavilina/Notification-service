namespace Api.Controllers.Messages.Responses;

public record class GetMessageResponse
{
    public required Guid Id { get; set; }

    public required string Channel { get; set; }

    public required string Recipient { get; set; } = null!;

    public required string? Subject { get; set; }

    public required string Message { get; set; } = null!;

    public required Dictionary<string, string> Metadata { get; set; }
}