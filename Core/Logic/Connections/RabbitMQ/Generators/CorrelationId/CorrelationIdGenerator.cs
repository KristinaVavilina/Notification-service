namespace Core.Logic.Connections.RabbitMQ.Generators.CorrelationId;

public class CorrelationIdGenerator : ICorrelationIdGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString();
    }
}