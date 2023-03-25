﻿using System;
using System.Linq;

namespace Kei.EventSourcing;

/// <summary>
/// Connects to the event store for saving <see cref="Event" /> in the correct order
/// and constructing instances of <see cref="AggregateRoot" /> by replaying all occured events for that aggregate
/// </summary>
public sealed class StateConnector
{
    private IEventStore _store;

    public StateConnector(IEventStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Get an instance of an <see cref="AggregateRoot" /> by replaying all events
    /// that occured for that aggregate
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="AggregateRoot" /> to retrieve</typeparam>
    /// <param name="aggregateRootId">The id of the <see cref="AggregateRoot"/> to retrieve</param>
    /// <returns>An instance of <see cref="AggregateRoot" /></returns>
    public T Get<T>(Guid aggregateRootId) where T : AggregateRoot, new()
    {
        T root = null;
        var allEvents = _store.Get(aggregateRootId)
            .ToList();

        if (allEvents.Any())
        {
            root = new T();

            root.FromHistory(allEvents);
        }

        return root;
    }

    /// <summary>
    /// Saves a new <see cref="Event"/> in the correct order.
    /// </summary>
    /// <param name="event">The <see cref="Event" /> to store.</param>
    public void Save(Event @event)
    {
        // Get the other events for this new event to determine the order.
        var otherEvents = _store.Get(@event.AggregateRootId);
        @event.Order = otherEvents.LastOrDefault()?.Order + 1 ?? 1;

        _store.Save(@event);
    }
}
