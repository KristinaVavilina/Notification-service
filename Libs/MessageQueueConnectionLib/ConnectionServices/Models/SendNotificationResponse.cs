public class SendNotificationResponse
{
    public bool IsSuccess { get; set; }

    public string? ErrorMessage { get; set; }

    public Guid MessageId { get; set; }

    public static SendNotificationResponse Success(Guid messageId)
    {
        return new SendNotificationResponse
        {
            IsSuccess = true,
            MessageId = messageId
        };
    }

    public static SendNotificationResponse Failure(string error, Guid messageId)
    {
        return new SendNotificationResponse
        {
            IsSuccess = false,
            ErrorMessage = error,
            MessageId = messageId
        };
    }
}