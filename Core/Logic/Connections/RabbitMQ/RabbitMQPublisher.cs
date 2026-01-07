using Core.Logic.Connections.RabbitMQ.Generators.CorrelationId;
using Core.Logic.Connections.RabbitMQ.Generators.QueueName;
using Core.Logic.Connections.RabbitMQ.Interfaces;
using Core.Logic.Connections.RabbitMQ.Models;
using Core.Logic.Serialization.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CallbackMapper = System.Collections.Generic.Dictionary<string, System.Threading.Tasks.TaskCompletionSource<byte[]>>;

namespace Core.Logic.Connections.RabbitMQ;

internal class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly string _responseQueueName;
    private readonly IModel _channel;
    private readonly ICorrelationIdGenerator _idGenerator;
    private readonly IQueueNameGenerator _nameGenerator;
    private readonly IMessageSerializer _serializer;
    private readonly CallbackMapper _mapper;

    public RabbitMQPublisher(ICorrelationIdGenerator idGenerator,
        IQueueNameGenerator nameGenerator,
        IMessageSerializer serializer,
        IRabbitMQConnectionFactory connection)
    {
        _idGenerator = idGenerator;
        _nameGenerator = nameGenerator;
        _mapper = new CallbackMapper();
        _serializer = serializer;
        _channel = connection.GetConnection().CreateModel();
        _responseQueueName = DeclareResponseQueue();
        StartListeningForResponses();
    }

    private void StartListeningForResponses()
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, args) =>
        {
            HandleResponseReceived(args);
            await Task.CompletedTask;
        };

        _channel.BasicConsume(consumer: consumer, queue: _responseQueueName, autoAck: true);
    }

    private void HandleResponseReceived(BasicDeliverEventArgs args)
    {
        var incomingId = args.BasicProperties.CorrelationId;
        Console.WriteLine($"[DEBUG] Пришел ответ с ID: {incomingId}");

        // Проверяем, есть ли такой ключ в нашем словаре
        if (_mapper.ContainsKey(incomingId))
        {
            Console.WriteLine("[SUCCESS] Ключ НАЙДЕН в Mapper! Извлекаем...");
        }
        else
        {
            // !!! Если вы видите это сообщение, значит у вас проблема "Чужого ответа" !!!
            Console.WriteLine($"[FAIL] Ключ НЕ НАЙДЕН в Mapper. В словаре сейчас ключей: {_mapper.Count}");

            // Вывод всех ключей, которые есть (чтобы сравнить)
            foreach (var key in _mapper.Keys)
            {
                Console.WriteLine($"   -> Ждем ID: {key}");
            }
            return;
        }

        if (!_mapper.Remove(args.BasicProperties.CorrelationId, out var tcs))
        {
            return;
        }
        tcs.TrySetResult(args.Body.ToArray());
    }

    private string DeclareResponseQueue()
    {
        var queueName = _channel.QueueDeclare().QueueName;
        return queueName;
    }

    public void Publish<T>(T message, IModel channel, PublishArguments arguments)
    {
        var body = _serializer.Serialize(message);
        channel.BasicPublish(arguments.ExchangeName, arguments.RoutingKey, arguments.Properties, body);
    }

    public async Task<TResponse?> SendAsync<TRequest, TResponse>(TRequest request, PublishArguments arguments, CancellationToken cancellationToken = default)
    {
        var properties = _channel.CreateBasicProperties();
        var correlationId = _idGenerator.Generate();

        _channel.ExchangeDeclare(exchange: arguments.ExchangeName, type: ExchangeType.Direct);
        _channel.QueueDeclare(queue: arguments.RoutingKey, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: arguments.RoutingKey, exchange: arguments.ExchangeName, routingKey: arguments.RoutingKey);

        properties.ReplyTo = _responseQueueName;
        properties.CorrelationId = correlationId;

        Publish(request, _channel, new PublishArguments
        {
            ExchangeName = arguments.ExchangeName,
            RoutingKey = arguments.RoutingKey,
            Properties = properties
        });
        var response = await WaitForResponseAsync<TResponse?>(correlationId, cancellationToken);
        return response;
    }

    private async Task<TResponse?> WaitForResponseAsync<TResponse>(string correlationId, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<byte[]>();
        bool isAdded = _mapper.TryAdd(correlationId, tcs);

        if (isAdded)
        {
            Console.WriteLine($"[DEBUG] ID добавлен в Mapper: {correlationId}");
        }
        else
        {
            Console.WriteLine($"[ERROR] Не удалось добавить ID в Mapper! Дубликат?: {correlationId}");
        }
        cancellationToken.Register(() => _mapper.Remove(correlationId));
        var responseData = await tcs.Task;

        return _serializer.Deserialize<TResponse>(responseData);
    }
}