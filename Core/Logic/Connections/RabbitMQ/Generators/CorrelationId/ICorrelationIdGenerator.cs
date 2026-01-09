namespace Core.Logic.Connections.RabbitMQ.Generators.CorrelationId;

public interface ICorrelationIdGenerator
{
    public string Generate();
}
