public class MessageRequest
{
    public Guid Id { get; set; }

    public required string ChannelType { get; set; }

    public required string Recipient { get; set; }

    public string? Subject { get; set; }

    public required string Content { get; set; }
}