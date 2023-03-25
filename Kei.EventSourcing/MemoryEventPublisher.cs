using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kei.EventSourcing.Interfaces;

namespace Kei.EventSourcing;

public sealed class MemoryEventPublisher : IEventPublisher, IEventSubscription
{
    private Dictionary<Type, List<Action<Event>>> _registrations;
    private object _registrationsLock;

    public MemoryEventPublisher()
    {
        _registrations = new Dictionary<Type, List<Action<Event>>>();
        _registrationsLock = new object();
    }

    /// <summary>
    /// Publish the occurence of <see cref="Event">event</see> to all subscribed components.
    /// </summary>
    /// <param name="event">The even t to publish</param>
    public Task PublishAsync(Event @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event), "Event should not be null");

        if (_registrations.ContainsKey(@event.GetType()))
        {
            var registeredActions = _registrations[@event.GetType()];

            foreach (var registeredAction in registeredActions)
            {
                registeredAction.Invoke(@event);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Allows a component to subscribe for event occurences
    /// </summary>
    /// <param name="event">The type of the event to subscribe on</param>
    /// <param name="@action">The action that will be executed once the event occurs</param>
    public IEventSubscription Subscribe(Type eventType, Action<Event> @action)
    {
        if (eventType.BaseType != typeof(Event)) throw new ArgumentException("Can only subscribe to Event types");

        lock(_registrationsLock)
        {
            if (!_registrations.ContainsKey(eventType))
            {
                _registrations[eventType] = new List<Action<Event>>();
            }

            _registrations[eventType].Add(@action);
        }

        return this;
    }
}
