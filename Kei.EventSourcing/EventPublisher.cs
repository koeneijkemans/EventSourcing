using System;
using System.Collections.Generic;

namespace Kei.EventSourcing
{
    public sealed class EventPublisher : ISubscription
    {
        private Dictionary<Type, List<Action<Event>>> _registrations;
        private object _registrationsLock;

        public EventPublisher()
        {
            _registrations = new Dictionary<Type, List<Action<Event>>>();
            _registrationsLock = new object();
        }

        /// <summary>
        /// Publish the occurence of <see cref="Event">event</see> to all subscribed components.
        /// </summary>
        /// <param name="event">The even t to publish</param>
        public void Publish(Event @event)
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
        }

        /// <summary>
        /// Allows a component to subscribe for event occurences
        /// </summary>
        /// <param name="event">The type of the event to subscribe on</param>
        /// <param name="@action">The action that will be executed once the event occurs</param>
        public ISubscription Subscribe(Type eventType, Action<Event> @action)
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
}
