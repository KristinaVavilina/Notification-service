using Core;

namespace Api.Controllers.Messages.Responses;

public record class GetStatusHistoryResponse
{
    public record class Entry
    {
        public required MessageStatus Status { get; init; }
        public required DateTime TimeStamp { get; init; }
    }

    public required Entry[] Entries { get; init; }
}
