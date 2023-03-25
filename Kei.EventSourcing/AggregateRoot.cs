using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kei.EventSourcing;

/// <summary>
/// The aggregate root is an object that defines an entity within the domain.
/// </summary>
public class AggregateRoot
{
    /// <summary>
    /// The id of this <see cref="AggregateRoot" />
    /// </summary>
    public Guid Id { get; set; }

    public AggregateRoot()
    {
    }

    /// <summary>
    /// Restore an <see cref="AggregateRoot"/> by applying all historic events to it.
    /// </summary>
    /// <param name="events">The events to apply to this <see cref="AggregateRoot" /> to re-contruct the object.</param>
    public void FromHistory(List<Event> events)
    {
        foreach (var @event in events)
        {
            ApplyEvent(@event);
            Id = @event.AggregateRootId;
        }
    }

    private void ApplyEvent(Event @event)
    {
        var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            var parameter = method.GetParameters().FirstOrDefault();
            if (parameter != null 
                && parameter.ParameterType == @event.GetType())
            {
                method.Invoke(this, new object[1] { @event });
                break;
            }
        }
    }
}
