using System;

namespace Kei.EventSourcing;

/// <summary>
/// Abstract base for all commands
/// </summary>
public abstract class Command
{
    public Guid AggregateRootId { get; set; }
}
