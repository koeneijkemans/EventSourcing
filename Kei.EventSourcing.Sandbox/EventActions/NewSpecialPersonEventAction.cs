using Kei.EventSourcing.Interfaces;
using Kei.EventSourcing.Sandbox.Events;
using Kei.EventSourcing.Sandbox.Read;

namespace Kei.EventSourcing.Sandbox.EventActions
{
    internal class NewSpecialPersonEventAction : IEventAction<PersonCreatedEvent>
    {
        private readonly List<SuperSpecialPersonModel> specialPersons;

        public NewSpecialPersonEventAction(List<SuperSpecialPersonModel> specialPersons)
        {
            this.specialPersons = specialPersons;
        }

        public void Action(PersonCreatedEvent @event)
        {
            specialPersons.Add(new SuperSpecialPersonModel()
            {
                Id = @event.AggregateRootId,
                Name = @event.Name,
                Age = @event.Age,
            });
        }
    }
}
