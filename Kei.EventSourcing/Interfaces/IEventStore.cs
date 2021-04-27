using System;
using System.Collections.Generic;

namespace Kei.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<Event> Get(Guid aggregateId);

        IEnumerable<Event> GetAll(params Type[] eventType);

        void Save(Event @event);
    }
}
