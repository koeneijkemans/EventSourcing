using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kei.EventSourcing
{
    /// <summary>
    /// This class allows for easy implementation of a place to store events.
    /// </summary>
    public abstract class EventStore : IEventStore
    {
        private readonly EventPublisher _eventPublisher;

        public EventStore(EventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Retrieve all events for the given id
        /// </summary>
        /// <param name="aggregateId">The id of the <see cref="AggregateRoot" /> to get the events for</param>
        /// <returns>A list of events that belong to the given <see cref="AggregateRoot" /></returns>
        public abstract IEnumerable<Event> Get(Guid aggregateId);

        /// <summary>
        /// Retrieve all events by event type
        /// </summary>
        /// <param name="eventTypes">The event types to retrieve</param>
        /// <returns>List of events by types.</returns>
        public abstract IEnumerable<Event> GetAll(params Type[] eventTypes);

        /// <summary>
        /// Saves the given event in the store.
        /// Once the save is completed will also publish the event on the <see cref="EventPublisher" />
        /// </summary>
        /// <param name="event">The <see cref="Event" /> to store</param>
        public void Save(Event @event)
        {
            SaveInStore(@event);

            _eventPublisher.Publish(@event);
        }

        /// <summary>
        /// Called by <see cref="EventStore.Save(@event)" />.
        /// This implementation should persist the <see cref="Event" /> in the store.
        /// </summary>
        /// <param name="event">The <see cref="Event" /> to store</param>
        protected abstract void SaveInStore(Event @event);

        protected Event ToEvent(IEventStoreItem item)
        {
            Type eventType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(item.EventType));

            var currentEvent = Activator.CreateInstance(eventType);
            JsonConvert.PopulateObject(item.Data, currentEvent);

            return currentEvent as Event;
        }
    }
}
