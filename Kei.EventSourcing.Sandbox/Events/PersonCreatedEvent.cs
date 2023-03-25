namespace Kei.EventSourcing.Sandbox.Events;

internal class PersonCreatedEvent : Event
{
    public string? Name { get; set; }

    public int Age { get; set; }
}
