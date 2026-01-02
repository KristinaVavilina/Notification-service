using System.Collections;

namespace Dal.Message.Models;

public record class MessageMetadataDal
{
    public required Guid MessageId { get; set; }
    public MessageDal Message { get; set; } = null!;

    public required string Key { get; set; } = null!;
    public required string Value { get; set; } = null!;
}