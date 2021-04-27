using System;
using System.Collections.Generic;
using System.Linq;

namespace Kei.EventSourcing
{
    /// <summary>
    /// A memory implementation of <see cref="EventStore" />.
    /// 
    /// This class is used for testing purposes and should only be used on production environments with serious cause.
    /// </summary>
    public sealed class MemoryEventStore : EventStore
    {
        private List<Event> _events;

        public MemoryEventStore(EventPublisher publisher)
            : base (publisher)
        {
            _events = new List<Event>();
        }

        public override IEnumerable<Event> Get(Guid aggregate)
        {
            return _events
                .Where(c => c.AggregateRootId == aggregate)
                .OrderBy(e => e.Order);
        }

        public override IEnumerable<Event> GetAll(params Type[] eventTypes)
        {
            return _events
                .Where(e => eventTypes.Contains(e.GetType()))
                .OrderBy(e => e.AggregateRootId).ThenBy(e => e.Order);
        }

        protected override void SaveInStore(Event @event)
        {
            _events.Add(@event);
        }
    }
}
