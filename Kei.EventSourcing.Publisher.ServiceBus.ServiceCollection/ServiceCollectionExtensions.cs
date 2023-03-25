using Kei.EventSourcing.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kei.EventSourcing.Publisher.ServiceBus.ServiceCollection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceBusPublisher(this IServiceCollection collection)
    {
        collection.AddOptions<ServiceBusOptions>();
        collection.AddTransient<IEventPublisher, ServiceBusPublisher>();

        return collection;
    }

    public static IServiceCollection AddServiceBusListener(this IServiceCollection collection)
    {
        collection.AddOptions<ServiceBusOptions>();
        collection.AddTransient<IEventListener, ServiceBusListener>();

        return collection;
    }
}
