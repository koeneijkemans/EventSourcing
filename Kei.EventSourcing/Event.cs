using System;

namespace Kei.EventSourcing;

/// <summary>
/// Base class for all events
/// </summary>
public class Event
{
    /// <summary>
    /// Id of the <see cref="AggregateRoot"/> this event is linked to.
    /// </summary>
    public Guid AggregateRootId { get; set; }

    /// <summary>
    /// The order this event should be processed in.
    /// </summary>
    public int Order { get; set; }
}
