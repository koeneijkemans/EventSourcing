namespace Kei.EventSourcing.Publisher.ServiceBus;

public class ServiceBusOptions
{
    public string? ConnectionString { get; set; }

    public string? QueueName { get; set; }
}
