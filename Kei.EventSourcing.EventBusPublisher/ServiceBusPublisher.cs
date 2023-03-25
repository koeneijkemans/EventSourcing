using Azure.Messaging.ServiceBus;
using Kei.EventSourcing.Interfaces;
using Newtonsoft.Json;

namespace Kei.EventSourcing.Publisher.ServiceBus;

public class ServiceBusPublisher : IEventPublisher
{
    private string _connectionString;
    private string _queueName;

    public ServiceBusPublisher(ServiceBusOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.ConnectionString);
        ArgumentNullException.ThrowIfNull(options.QueueName);

        _connectionString = options.ConnectionString;
        _queueName = options.QueueName;
    }

    public async Task PublishAsync(Event @event)
    {
        ServiceBusClient client = new ServiceBusClient(_connectionString);
        ServiceBusSender serviceBusSender = client.CreateSender(_queueName);

        ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(@event));
        message.Subject = @event.GetType().Name;

        await serviceBusSender.SendMessageAsync(message);
    }
}
