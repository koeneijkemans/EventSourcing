namespace Kei.EventSourcing.Sandbox.Events;

internal class NameChangedEvent : Event
{
    public string? Name { get; set; }
}
