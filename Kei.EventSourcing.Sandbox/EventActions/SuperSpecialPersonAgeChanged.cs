using Kei.EventSourcing.Interfaces;
using Kei.EventSourcing.Sandbox.Events;
using Kei.EventSourcing.Sandbox.Read;

namespace Kei.EventSourcing.Sandbox.EventActions
{
    internal class SuperSpecialPersonAgeChanged : IEventAction<AgeChangedEvent>
    {
        private readonly List<SuperSpecialPersonModel> superSpecialPersonModels;

        public SuperSpecialPersonAgeChanged(List<SuperSpecialPersonModel> superSpecialPersonModels)
        {
            this.superSpecialPersonModels = superSpecialPersonModels;
        }

        public void Action(AgeChangedEvent @event)
        {
            var p = superSpecialPersonModels.First(p => p.Id == @event.AggregateRootId);

            if (p != null) p.Age = @event.Age * 2;
        }
    }
}
