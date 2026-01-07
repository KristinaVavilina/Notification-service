namespace Core.Logic.Connections.RabbitMQ.Generators.QueueName;

public interface IQueueNameGenerator
{
    public string GenerateForRequests<T>();

    public string GenerateForResponses();
}
