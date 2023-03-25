using System;
using System.Collections.Generic;

namespace Kei.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<Event> Get(Guid aggregateId);

        void Save(Event @event);
    }
}
