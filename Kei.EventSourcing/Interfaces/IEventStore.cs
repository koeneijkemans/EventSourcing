using System;
using System.Collections.Generic;

namespace Kei.EventSourcing
{
    public interface IEventStore
    {
        List<Event> Get(Guid aggregateId);

        void Save(Event @event);
    }
}
