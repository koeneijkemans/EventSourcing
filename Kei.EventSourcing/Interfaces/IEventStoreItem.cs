using System;

namespace Kei.EventSourcing
{
    public interface IEventStoreItem
    {
        Guid AggregateId { get; set; }

        string EventType { get; set; }

        int Order { get; set; }

        string Data { get; set; }
    }
}
