using System;

namespace Kei.EventSourcing;

public interface IEventSubscription
{
    IEventSubscription Subscribe(Type eventType, Action<Event> @action);
}
