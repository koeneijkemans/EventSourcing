using Kei.EventSourcing.Sandbox.Events;

namespace Kei.EventSourcing.Sandbox.Domain;

internal class Person : AggregateRoot
{
    public string? Name { get; set; }

    public int Age { get; set; }

    private void Create(PersonCreatedEvent @event)
    {
        Name = @event.Name;
        Age = @event.Age;
        Id = @event.AggregateRootId;
    }

    private void ChangeName(NameChangedEvent @event)
    {
        Name = @event.Name;
    }

    private void ChangeAge(AgeChangedEvent @event)
    {
        Age = @event.Age;
    }

    public override string ToString()
    {
        return $"{Name}, {Age}";
    }
}
