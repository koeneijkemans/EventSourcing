using Azure.Messaging.ServiceBus;
using Kei.EventSourcing.Interfaces;
using Newtonsoft.Json;

namespace Kei.EventSourcing.Publisher.ServiceBus;

public class ServiceBusListener : IEventListener
{
    private readonly EventSubscriptionManager _subscriptionManager;
    private ServiceBusProcessor _processor;

    public ServiceBusListener(
        EventSubscriptionManager subscriptionManager,
        ServiceBusOptions options)
    {
        ArgumentNullException.ThrowIfNull(subscriptionManager, nameof(subscriptionManager));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        ArgumentNullException.ThrowIfNull(options.QueueName);
        ArgumentNullException.ThrowIfNull(options.ConnectionString);

        ServiceBusClient client = new ServiceBusClient(options.ConnectionString);
        _processor = client.CreateProcessor(options.QueueName);
        _subscriptionManager = subscriptionManager;
    }

    public async Task StartListening()
    {
        _processor.ProcessMessageAsync += (ProcessMessageEventArgs arg) =>
        {
            ServiceBusReceivedMessage message = arg.Message;
            if (message != null
                && !string.IsNullOrEmpty(message.Subject)
                && _subscriptionManager.EventMap.TryGetValue(message.Subject, out List<Action<Event>>? actionsToExecute))
            {
                Type? eventType = AppDomain.CurrentDomain.GetAssemblies()
                   .Where(a => !a.IsDynamic)
                   .SelectMany(a => a.GetTypes())
                   .FirstOrDefault(t => t.Name.Equals(message.Subject));

                if (eventType == null) return Task.CompletedTask;

                object @event = (Event)Activator.CreateInstance(eventType)!;
                JsonConvert.PopulateObject(message.Body.ToString(), @event);

                foreach (var action in actionsToExecute)
                {
                    action.Invoke((Event)@event);
                }
            }

            return Task.CompletedTask;
        };

        _processor.ProcessErrorAsync += (ProcessErrorEventArgs arg) => Task.CompletedTask;

        await _processor.StartProcessingAsync();
    }
}
