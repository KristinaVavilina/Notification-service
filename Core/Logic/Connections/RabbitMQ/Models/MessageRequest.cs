public class MessageRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string ChannelType { get; set; }

    public required string Recipient { get; set; }

    public string? Subject { get; set; }

    public required string Content { get; set; }

    // Нужно?
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}