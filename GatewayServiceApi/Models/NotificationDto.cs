namespace GatewayServiceApi.Models;

public class NotificationDto
{
    public required string ChannelType { get; set; }

    public required string Recipient { get; set; }

    public string? Subject { get; set; }

    public required string Content { get; set; }

    public required Dictionary<string, string> Metadata { get; set; }
}
