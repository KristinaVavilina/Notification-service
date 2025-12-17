public class NotificationMessage
{
    public Guid Id { get; set; }

    public required string ChannelType { get; set; } // email, sms, push

    public required string Recipient { get; set; }   // email или номер телефона

    public string? Subject { get; set; }

    public required string Content { get; set; }
}