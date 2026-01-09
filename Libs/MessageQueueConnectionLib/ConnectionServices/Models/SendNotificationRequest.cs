public class SendNotificationRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string ChannelType { get; set; }

    public required string Recipient { get; set; }

    public string? Subject { get; set; }

    public required string Content { get; set; }

    public required Dictionary<string, string> Metadata { get; set; }
}