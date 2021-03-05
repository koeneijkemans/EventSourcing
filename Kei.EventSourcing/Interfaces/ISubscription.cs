using System;

namespace Kei.EventSourcing
{
    public interface ISubscription
    {
        ISubscription Subscribe(Type eventType, Action<Event> @action);
    }
}
