namespace Core.Logic.Connections.RabbitMQ.Generators.QueueName;

public class QueueNameGenerator : IQueueNameGenerator
{
    public string GenerateForRequests<T>()
    {
        return typeof(T).Name;
    }

    public string GenerateForResponses()
    {
        return "Responses";
    }
}